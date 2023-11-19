using Microsoft.EntityFrameworkCore;
using StudyHub.DAL.EF;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.DAL.Repositories;
using StudyHub.Common.Models;
using StudyHub.BLL.Services.Interface;
using StudyHub.BLL.Services;
using StudyHub.BLL.Profiles;
using StudyHub.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAutoMapper(typeof(AssignmentTaskProfile));

builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Service
builder.Services.AddScoped<IAssignmentTaskService, AssignmentTaskService>();
builder.Services.AddScoped<IOptionsService, OptionsService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
