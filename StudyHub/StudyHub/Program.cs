using Microsoft.EntityFrameworkCore;
using StudyHub.DAL.EF;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.DAL.Repositories;
using StudyHub.Common.Models;
using StudyHub.BLL.Services;
using StudyHub.BLL.Profiles;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Entities;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using StudyHub.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using StudyHub.Validators.AssignmentTaskOptionValidators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAutoMapper(typeof(AssignmentTaskProfile));

builder.Services.AddControllers(cfg => cfg.Filters.Add(typeof(ExceptionFilter)));

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

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAssignmentTaskOptionValidator>();
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:Secret")!)),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = false,
    ValidateLifetime = true
};
builder.Services.AddAuthentication(configureOptions: x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.SaveToken = true;
        x.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddSingleton(tokenValidationParameters);

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
