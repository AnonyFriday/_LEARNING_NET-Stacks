using Gender.SoapClients.MVCWebApp.DuyVK.TokenHandlers;
using MenstrualCycleReminderDuyVKServiceReference;
using Microsoft.AspNetCore.Authentication.Cookies;
using ReminderCategoryDuyVKServiceReference;
using SystemUserAccountDuyVKServiceReference;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configure Authentication
builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/Forbidden");
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

    });

builder.Services.AddScoped<SystemUserAccountSoapServiceClient>();

// Register a factory to add the BearerTokenBehavior
builder.Services.AddScoped<BearerTokenBehavior>(sp =>
{
    var httpCtx = sp.GetRequiredService<IHttpContextAccessor>();
    return new BearerTokenBehavior(() =>
        httpCtx.HttpContext?.Request.Cookies["TokenString"] ?? ""
    );
});

// Configure each client to use the BearerTokenBehavior
builder.Services.AddScoped<ReminderCategoryDuyVKSoapServiceClient>(sp =>
{
    var client = new ReminderCategoryDuyVKSoapServiceClient();
    var behavior = sp.GetRequiredService<BearerTokenBehavior>();
    client.Endpoint.EndpointBehaviors.Add(behavior);
    return client;
});

builder.Services.AddScoped<MenstrualCycleReminderDuyVKSoapServiceClient>(sp =>
{
    var client = new MenstrualCycleReminderDuyVKSoapServiceClient();
    var behavior = sp.GetRequiredService<BearerTokenBehavior>();
    client.Endpoint.EndpointBehaviors.Add(behavior);
    return client;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();
