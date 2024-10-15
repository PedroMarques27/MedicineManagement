using Database;
using Database.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Process.DTOs.Entities;
using Process.Profiles;
using Process.Providers;
using Process.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 30))
        )
    );

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile)); // Adjust if necessary

builder.Services.AddScoped<IUsersProvider, UsersProvider>();
builder.Services.AddScoped<IMedicineProvider, MedicineProvider>();
builder.Services.AddScoped<IPrescriptionProvider, PrescriptionProvider>();

builder.Services.AddControllers();

builder.Services.AddScoped<IValidator<Medicine>, MedicineValidator>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();


app.MapControllers();
app.Run();
