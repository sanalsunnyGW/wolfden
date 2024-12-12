using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using WolfDen.Application.Helper.LeaveManagement;
using WolfDen.Application.Helpers;
using WolfDen.Application.Requests.Commands.Attendence.Service;
using WolfDen.Application.Requests.Queries.Attendence.DailyDetails;
using WolfDen.Application.Requests.Queries.Attendence.MonthlyReport;
using WolfDen.Application.Requests.Queries.Attendence.SendWeeklyEmail;
using WolfDen.Application.Services;
using WolfDen.Domain.ConfigurationModel;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add FluentValidation services to the container
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

// Add your services here
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization:`Bearer Genetrated-JWT_Token`",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme,
                }
            },
            new string[] { }
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WolfDenContext>();

builder.Services.Configure<JwtKey>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<OfficeDurationSettings>(builder.Configuration.GetSection("OfficeDuration"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader();
                      });
});

builder.Services.AddDbContext<WolfDenContext>(x =>
{
    x.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(x =>
{
    x.Password.RequiredLength = 5;
    x.Password.RequireUppercase = true;
    x.Password.RequireLowercase = true;
    x.Password.RequireDigit = true;
}).AddEntityFrameworkStores<WolfDenContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        RoleClaimType = ClaimTypes.Role,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"]

    };
});

builder.Services.AddScoped<WolfDenContext>();
builder.Services.AddSingleton<PdfService>();
builder.Services.AddScoped<ManagerEmailFinder>();
builder.Services.AddScoped<ManagerIdFinder>();
builder.Services.AddScoped<MonthlyPdf>();
builder.Services.AddScoped<Email>();
builder.Services.AddSingleton<WeeklyPdfService>();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssembly(Assembly.Load("WolfDen.Application"));
});

builder.Services.AddValidatorsFromAssembly(Assembly.Load("WolfDen.Application"));

Console.WriteLine("Started Succesfully");

builder.Services.AddHangfire(configuration => configuration
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseInMemoryStorage());
Console.WriteLine("Added Succesfully");

builder.Services.AddHangfireServer();
builder.Services.AddScoped(sp =>
            new QueryBasedSyncService(
                builder.Configuration.GetConnectionString("BioMetricDatabase"),
                connectionString,
                sp.GetRequiredService<ILogger<QueryBasedSyncService>>()
            ));
builder.Services.AddScoped<DailyAttendancePollerService>();
builder.Services.AddScoped<WeeklyAttendancePollerService>();

builder.Services.AddControllers();

var app = builder.Build();

// FluentValidation error handling middleware
app.UseMiddleware<ValidationExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


using (var scope = app.Services.CreateScope())
{
    var syncService = scope.ServiceProvider.GetRequiredService<QueryBasedSyncService>();
    var combineService = scope.ServiceProvider.GetRequiredService<DailyAttendancePollerService>();
    var weeklyService= scope.ServiceProvider.GetRequiredService<WeeklyAttendancePollerService>();

    RecurringJob.AddOrUpdate(
        "sync-tables-job",
        () => syncService.SyncTablesAsync(),
        "*/5 * * * *"  // Cron expression for every 5 minutes
    );


    RecurringJob.AddOrUpdate(
        "send-attendance-notifications-job",
        () => combineService.ExecuteJobAsync(),
        "0 0 * * 2-6"
    );
    
    RecurringJob.AddOrUpdate(
     "send-weeklyemails-job",
     () => weeklyService.WeeklyEmail(),
     "0 0 * * 6"

 );
}

// Custom middleware to handle FluentValidation exceptions
public class ValidationExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 400;
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
            var errorResponse = new { Errors = errors };
            await httpContext.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
