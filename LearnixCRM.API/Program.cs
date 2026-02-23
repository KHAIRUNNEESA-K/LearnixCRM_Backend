using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using LearnixCRM.API.Middleware;
using LearnixCRM.Application.Auth.Services;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Application.Mapping;
using LearnixCRM.Application.MappingProfiles;
using LearnixCRM.Application.Mappings;
using LearnixCRM.Application.Services;
using LearnixCRM.Application.Validators;
using LearnixCRM.Infrastructure.Configuration;
using LearnixCRM.Infrastructure.ExternalServices;
using LearnixCRM.Infrastructure.Persistence.Context;
using LearnixCRM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;


Env.Load();

var builder = WebApplication.CreateBuilder(args);
 builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });



builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LearnixCRM API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
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
    });
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

builder.Services.AddDbContext<LearnixDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDbConnection>(_ =>
    new SqlConnection(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(SetPasswordMappingProfile));
builder.Services.AddAutoMapper(typeof(UserProfileMapping));
builder.Services.AddAutoMapper(typeof(RegisterUserMappingProfile));
builder.Services.AddAutoMapper(typeof(LeadMappingProfile));
builder.Services.AddAutoMapper(typeof(FollowUpMappingProfile));
builder.Services.AddAutoMapper(typeof(BlacklistMappingProfile));

// ===== Repositories =====
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISetPasswordRepository, SetPasswordRepository>();
builder.Services.AddScoped<IAssignUsersRepository, AssignUsersRepository>();
builder.Services.AddScoped<IAssignedSalesRepository, AssignedSalesRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<ISalesLeadRepository, SalesLeadRepository>();
builder.Services.AddScoped<ISalesFollowUpRepository,SalesFollowUpRepository>();
builder.Services.AddScoped<IBlacklistRepository, BlacklistRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// ===== Services =====
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, AuthTokenService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<ISetPasswordService, SetPasswordService>();
builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
builder.Services.AddScoped<IAssignUsersService, AssignUsersService>();
builder.Services.AddScoped<IAssignedSalesService, AssignedSalesService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<ISalesLeadService, SalesLeadService>();
builder.Services.AddScoped<ISalesFollowUpService,SalesFollowUpService>();
builder.Services.AddScoped<IBlacklistService, BlacklistService>();
builder.Services.AddScoped<IStudentService, StudentService>();

// ===== Hosted Services =====
builder.Services.AddHostedService<TokenCleanupService>();



builder.Services.Configure<SendGridSettings>(
    builder.Configuration.GetSection("SendGrid")
);


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("1"));

    options.AddPolicy("ManagerOnly", policy =>
        policy.RequireRole("2"));

    options.AddPolicy("SalesOnly", policy =>
        policy.RequireRole("3"));
});
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

builder.Configuration.AddEnvironmentVariables();

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()
    ?? throw new Exception("JwtSettings missing from environment");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT AUTH FAILED:");
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.Configure<CloudinarySettings>(options =>
{
    options.CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME")!;
    options.ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY")!;
    options.ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")!;
});

builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
