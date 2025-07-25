using Gender.Repositories.DuyVK;
using Gender.Repositories.DuyVK.DBContext;
using Gender.Services.DuyVK;
using Gender.Services.DuyVK.Interfaces;
using Gender.SoapApiServices.DuyVK.SoapServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SoapCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<GenderContext>();

// Add Services classes 
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddScoped<IMenstrualCycleReminderDuyVKService, MenstrualCycleReminderDuyVKService>();
builder.Services.AddScoped<ISystemUserAccountService, SystemUserAccountService>();
builder.Services.AddScoped<IReminderCategoryDuyVKService, ReminderCategoryDuyVKService>();

// Add Repositories classes
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMenstrualCycleReminderDuyVKRepository, MenstrualCycleReminderDuyVKRepository>();
builder.Services.AddScoped<ISystemUserAccountRepository, SystemUserAccountRepository>();
builder.Services.AddScoped<IReminderCategoryDuyVKRepository, ReminderCategoryDuyVKRepository>();

// Add SOAP Services classes
builder.Services.AddScoped<IMenstrualCycleReminderDuyVKSoapService, MenstrualCycleReminderDuyVKSoapService>();
builder.Services.AddScoped<IReminderCategoryDuyVKSoapService, ReminderCategoryDuyVKSoapService>();
builder.Services.AddScoped<ISystemUserAccountSoapService, SystemUserAccountDuyVKSoapService>();

// Add Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SOAP Service
builder.Services.AddSoapCore();

// Add HttpContext Accessor for accessing the Claims Principal
builder.Services.AddHttpContextAccessor();

// Add JWT Authentication
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Authentication Middleware
app.UseAuthentication();

// Add Routing for endpoint matching
// - used with use endpoints
app.UseRouting();

// Add Authorization Middleware
app.UseAuthorization();

app.UseSoapEndpoint<IMenstrualCycleReminderDuyVKSoapService>(
    "/soap/MenstrualCycleReminderDuyVK.asmx",
    new SoapEncoderOptions()
);

app.UseSoapEndpoint<IReminderCategoryDuyVKSoapService>(
    "/soap/ReminderCategoryDuyVK.asmx",
    new SoapEncoderOptions()
);

app.UseSoapEndpoint<ISystemUserAccountSoapService>(
    "/soap/SystemUserAccountDuyVK.asmx",
    new SoapEncoderOptions()
);

// Add SOAP Endpoint (Recommend this configuration for SOAP services)
#pragma warning disable ASP0014
//app.UseEndpoints(endpoints =>
//{
//    endpoints.UseSoapEndpoint<IMenstrualCycleReminderDuyVKSoapService>(
//        "/soap/MenstrualCycleReminderDuyVK.asmx",
//        new BasicHttpBinding(BasicHttpSecurityMode.Transport),
//        SoapSerializer.XmlSerializer);

//    endpoints.UseSoapEndpoint<IReminderCategoryDuyVKSoapService>(
//        "/soap/ReminderCategoryDuyVK.asmx",
//        new SoapEncoderOptions(),
//        SoapSerializer.XmlSerializer);

//    endpoints.UseSoapEndpoint<ISystemUserAccountSoapService>(
//        "/soap/SystemUserAccountDuyVK.asmx",
//        new SoapEncoderOptions(),
//        SoapSerializer.XmlSerializer);

//    endpoints.MapControllers();
//});

app.Run();
