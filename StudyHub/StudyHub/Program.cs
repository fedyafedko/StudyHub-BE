using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudyHub.BLL.Profiles;
using StudyHub.BLL.Services;
using StudyHub.BLL.Services.Auth;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.BLL.Services.Interfaces.Auth;
using StudyHub.DAL.EF;
using StudyHub.DAL.Repositories;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using StudyHub.Extensions;
using StudyHub.FluentEmail.Services;
using StudyHub.FluentEmail.Services.Interfaces;
using StudyHub.Middlewares;
using StudyHub.Seeding.Extentions;
using StudyHub.Validators.AssignmentTaskOptionValidators;
using System.Text;
using Hangfire;
using StudyHub.Hangfire.Extensions;
using StudyHub.Common.Configs;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using StudyHub.Utility;
using StudyHub.BLL.Services.Interfaces.Assignment;
using StudyHub.BLL.Services.Assignments;
using Microsoft.OpenApi.Any;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigsAssembly(builder.Configuration, opt => opt
       .AddConfig<JwtConfig>()
       .AddConfig<GoogleAuthConfig>()
       .AddConfig<EmailConfig>()
       .AddConfig<HangfireConfig>()
       .AddConfig<UserInvitationConfig>());

builder.Services.AddAutoMapper(typeof(AssignmentTaskProfile));

builder.Services.AddControllers(cfg => cfg.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())));

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Hangfire
builder.Services.AddHangfire(builder.Configuration);

// Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Service
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IAssignmentTaskService, AssignmentTaskService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IVariantService, VariantService>();
builder.Services.AddScoped<IOptionsService, OptionsService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserInvitationService, UserInvitationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IEncryptService, EncryptService>();

// Fluent Email
builder.Services.AddFluentEmail(builder.Configuration);

// Seeding
builder.Services.AddSeeding();

// Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskOptionValidator>();
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtConfig:Secret")!)),
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
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
    c.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("00:00:00")
    });
});

// CORS
builder.Services.AddCors(options => options
    .AddDefaultPolicy(build => build
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.ApplySeedingAsync();
app.SetupHangfire();

app.UseHttpsRedirection();
app.UseCors(
    opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHangfireDashboard("/hangfire");

app.Run();