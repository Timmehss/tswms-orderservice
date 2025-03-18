#region Usings

using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

#endregion

var builder = WebApplication.CreateBuilder(args);

// Get Environment
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Configure CORS Policy
builder.Services.AddCors(o => o.AddPolicy("TSWMSPolicy", builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .SetIsOriginAllowed((host) => true)
           .AllowCredentials();
}));

var routes = "Routes";

// Load environment specific configuration
builder.Configuration
    .AddJsonFile($"appsettings.{environment}.json")
    .AddOcelotWithSwaggerSupport(options =>
    {
        options.Folder = $"{routes}/{environment}";
    });

builder.Services
    .AddOcelot(builder.Configuration)
    .AddPolly();

builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use CORS
app.UseCors("TSWMSPolicy");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseOcelot().Wait();

app.Run();
