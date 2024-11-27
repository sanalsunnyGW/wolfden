using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Reflection;
using System.Text;
using WolfDen.Domain.ConfigurationModel;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;
using WolfDen.Application.Requests.Queries.Attendence.DailyDetails;
using WolfDen.Application.Helpers;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
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
    };
});

builder.Services.AddScoped<WolfDenContext>();
builder.Services.AddSingleton<PdfService>();
builder.Services.AddScoped<ManagerEmailFinder>();
    
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssembly(Assembly.Load("WolfDen.Application"));

});
builder.Services.AddValidatorsFromAssembly(Assembly.Load("WolfDen.Application"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
