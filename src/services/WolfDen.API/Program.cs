using System.Reflection;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using WolfDen.Application.Helpers;
using WolfDen.Application.Requests.Commands.Attendence.Service;
using System.Configuration;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using WolfDen.Application.Helpers;
using WolfDen.Application.Requests.Queries.Attendence.DailyDetails;
using WolfDen.Application.Requests.Queries.Attendence.MonthlyReport;
using WolfDen.Domain.ConfigurationModel;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
        ValidateIssuer = false,
        ValidateAudience = false,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddScoped<WolfDenContext>();
builder.Services.AddSingleton<PdfService>();
builder.Services.AddScoped<ManagerEmailFinder>();
builder.Services.AddScoped<MonthlyPdf>();
//builder.Services.AddHostedService<DailyAttendancePollerService>();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssembly(Assembly.Load("WolfDen.Application"));

});
builder.Services.AddValidatorsFromAssembly(Assembly.Load("WolfDen.Application"));
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));
builder.Services.AddHangfireServer();
builder.Services.AddScoped(sp =>
            new QueryBasedSyncService(
                builder.Configuration.GetConnectionString("BioMetricDatabase"),
                connectionString,
                sp.GetRequiredService<ILogger<QueryBasedSyncService>>()
            ));
builder.Services.AddScoped<DailyAttendancePollerService>();

builder.Services.AddControllers();
var app = builder.Build();

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

using (var scope = app.Services.CreateScope())
{
    var syncService = scope.ServiceProvider.GetRequiredService<QueryBasedSyncService>();
    var emailService = scope.ServiceProvider.GetRequiredService<DailyAttendancePollerService>();

    RecurringJob.AddOrUpdate(
        "sync-tables-job",
        () => syncService.SyncTablesAsync(),
        "*/5 * * * *"  // Cron expression for every 5 minutes
    );
    RecurringJob.AddOrUpdate(
       "send-emails-job",
       () => emailService.SendEmail(),
       "0 0 * * 2-6"

   );
}

app.Run();
