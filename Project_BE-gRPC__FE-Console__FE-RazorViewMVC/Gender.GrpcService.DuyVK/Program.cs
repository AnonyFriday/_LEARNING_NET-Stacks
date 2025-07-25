using Gender.GrpcService.DuyVK.Services;
using Gender.Repositories.DuyVK;
using Gender.Repositories.DuyVK.DBContext;
using Gender.Services.DuyVK;
using Gender.Services.DuyVK.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Grpc 
builder.Services.AddGrpc(otps =>
{
    otps.EnableDetailedErrors = true;
});

// Add Services
builder.Services.AddDbContext<GenderContext>();
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddScoped<IMenstrualCycleReminderDuyVKService, MenstrualCycleReminderDuyVKService>();
builder.Services.AddScoped<ISystemUserAccountService, SystemUserAccountService>();
builder.Services.AddScoped<IReminderCategoryDuyVKService, ReminderCategoryDuyVKService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMenstrualCycleReminderDuyVKRepository, MenstrualCycleReminderDuyVKRepository>();
builder.Services.AddScoped<ISystemUserAccountRepository, SystemUserAccountRepository>();
builder.Services.AddScoped<IReminderCategoryDuyVKRepository, ReminderCategoryDuyVKRepository>();

// Add Authentication and Authorization
builder.Services
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero,
        };
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapGrpcService<MenstrualCycleReminderDuyVKGRPCService>();
app.MapGrpcService<ReminderCategoryDuyVKGRPCService>();
app.MapGrpcService<AuthDuyVKGRPCService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
