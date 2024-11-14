using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WolfDen.Application.Validators;
using WolfDen.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<WolfDenContext>(x =>
{
    x.UseSqlServer(connectionString);

});
builder.Services.AddScoped<WolfDenContext>();

builder.Services.AddMediatR(x => {
    x.RegisterServicesFromAssembly(Assembly.Load("WolfDen.Application"));

});

builder.Services.AddTransient<AdminUpdateEmployeeValidator>();
builder.Services.AddTransient<CreateEmployeeValidator>();
builder.Services.AddTransient<EmployeeUpdateEmployeeValidator>();



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
