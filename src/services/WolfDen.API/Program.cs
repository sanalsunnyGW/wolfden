using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Client;
using WolfDen.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
Configuration.Default.ApiKey.Add("api-key", builder.Configuration["BrevoApi:ApiKey"]);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WolfDenContext>(x =>
{
    x.UseSqlServer(@"Server=localhost,1431;Database=EmployeeManagement;User Id=sa;Password=pass@123;TrustServerCertificate=true");

});
builder.Services.AddScoped<WolfDenContext>();
builder.Services.AddMediatR(x => {
    x.RegisterServicesFromAssembly(Assembly.Load("WolfDen.Application"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
