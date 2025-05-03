using HealthMed.CommandAPI.Interfaces.Repository;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.CommandAPI.Repositories;
using HealthMed.CommandAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();