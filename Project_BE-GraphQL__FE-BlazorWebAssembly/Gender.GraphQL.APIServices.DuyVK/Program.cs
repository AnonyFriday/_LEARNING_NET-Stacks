using Gender.GraphQL.APIServices.DuyVK.GraphQLs;
using Gender.Repositories.DuyVK;
using Gender.Repositories.DuyVK.Basic;
using Gender.Repositories.DuyVK.DBContext;
using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Services.DuyVK;
using Gender.Services.DuyVK.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddDbContext<GenderContext>();

builder.Services.AddScoped<Queries>();
builder.Services.AddScoped<Mutations>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddScoped<IMenstrualCycleReminderDuyVKRepository, MenstrualCycleReminderDuyVKRepository>();
builder.Services.AddScoped<IReminderCategoryDuyVKRepository, ReminderCategoryDuyVKRepository>();
builder.Services.AddScoped<ISystemUserAccountRepository, SystemUserAccountRepository>();
builder.Services.AddScoped<IMenstrualCycleReminderDuyVKService, MenstrualCycleReminderDuyVKService>();
builder.Services.AddScoped<IReminderCategoryDuyVKService, ReminderCategoryDuyVKService>();
builder.Services.AddScoped<ISystemUserAccountService, SystemUserAccountService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();

// Add GraphQL Server
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Queries>()
    .AddMutationType<Mutations>()
    .AddType<InputObjectType<SearchMenstrualCycleReminderRequest>>()
    .BindRuntimeType<DateTime, DateTimeType>()
    .AddAuthorization();

// Add Json Options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});

// Add this one for authorization & authentication
builder.Services
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
            ClockSkew = TimeSpan.Zero
        };
    });

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

app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseRouting().UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();
