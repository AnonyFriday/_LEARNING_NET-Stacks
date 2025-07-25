using Blazored.LocalStorage;
using Gender.GraphQLClient.BlazorWAS.DuyVK;
using Gender.GraphQLClient.BlazorWAS.DuyVK.Auth;
using Gender.GraphQLClient.BlazorWAS.DuyVK.GraphQLClients;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// IGraphQLClient for calling GraphQL API Server
//builder.Services.AddScoped<IGraphQLClient>(c => new GraphQLHttpClient(builder.Configuration["GraphQLURI"], new NewtonsoftJsonSerializer()));
builder.Services.AddTransient<AuthHeaderHandler>();
builder.Services.AddScoped<IGraphQLClient>(sp =>
{
    // Get an instance of your AuthorizationHeaderHandler
    var authHeaderHandler = sp.GetRequiredService<AuthHeaderHandler>();

    // Set the InnerHandler for the AuthorizationHeaderHandler
    authHeaderHandler.InnerHandler = new HttpClientHandler();

    // Create an HttpClient that uses your AuthorizationHeaderHandler
    var httpClient = new HttpClient(authHeaderHandler)
    {
        BaseAddress = new Uri(builder.Configuration["GraphQLURI"])
    };

    // Create the GraphQLHttpClient
    return new GraphQLHttpClient(
        builder.Configuration["GraphQLURI"],
        new NewtonsoftJsonSerializer(),
        httpClient
    );
});


// HttpClient for GraphQL requests
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Service
builder.Services.AddScoped<MenstrualCycleReminderDuyVKGraphQLConsumer>();
builder.Services.AddScoped<ReminderCategoryDuyVKGraphQLConsumer>();
builder.Services.AddScoped<AuthenticationGraphQLConsumercs>();

// Local storage & auth-state provider
builder.Services.AddBlazoredLocalStorageAsSingleton(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddScoped<JwtAuthenticationStateProvider>();

await builder.Build().RunAsync();


