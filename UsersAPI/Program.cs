using Application;
using Application.Interfaces;
using Application.Services;
using Domain.Common.Settings;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Context;
using Infrastructure.Messaging.Producers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FGC API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Autenticação JWT usando Bearer Token. 
                      
            Para usar, copie o token recebido no login e cole no campo abaixo. 
            O sistema adicionará automaticamente 'Bearer' no início do token.

            Exemplo: se seu token é 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...', 
            cole apenas 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...' (sem aspas, sem 'Bearer').",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<FCGDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddMassTransit(x =>
{


    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            builder.Configuration["RabbitMQ:Host"],
            ushort.Parse(builder.Configuration["RabbitMQ:Port"]!),
            "/",
            h =>
            {
                h.Username(
                    builder.Configuration["RabbitMQ:Username"]);

                h.Password(
                    builder.Configuration["RabbitMQ:Password"]);
            });



    });
});

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName);

var jwtConfig = jwtSettings.Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings não configurado.");

var key = Encoding.UTF8.GetBytes(jwtConfig.SecretKey);

builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options => {
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = jwtConfig.ValidateIssuer,
        ValidIssuer = jwtConfig.Issuer,
        ValidateAudience = jwtConfig.ValidateAudience,
        ValidAudience = jwtConfig.Audience,
        ValidateLifetime = jwtConfig.ValidateLifetime,
        ClockSkew = TimeSpan.FromMinutes(jwtConfig.ClockSkew)
    };

});

builder.Services.AddApplicationServices();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserCreatedProducer, UserCreatedProducer>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FCGDbContext>();

    db.Database.Migrate();
}

app.Run();
