using Microsoft.EntityFrameworkCore;
using StudyHub.DAL.EF;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.DAL.Repositories;
using StudyHub.Common.Models;
using StudyHub.BLL.Services.Interface;
using StudyHub.BLL.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Service
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
