using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using sib_api_v3_sdk.Client;
using WolfDen.Application.Requests.Queries.Attendence.DailyAttendanceReport;
using WolfDen.Application.Requests.Queries.Attendence.MonthlyReport;
using WolfDen.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

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
builder.Services.AddScoped<WolfDenContext>();
builder.Services.AddSingleton<PdfService>();
builder.Services.AddScoped<MonthlyPdf>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
