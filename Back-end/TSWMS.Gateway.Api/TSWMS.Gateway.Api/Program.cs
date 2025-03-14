#region Usings

using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using TSWMS.Gateway.Api;

#endregion

var builder = WebApplication.CreateBuilder(args);

var routes = "Routes";

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

// Configure App Configuration
builder.Configuration
    .AddJsonFile($"appsettings.{environment}.json")
    .AddOcelotWithSwaggerSupport(options =>
    {
        options.Folder = $"{routes}/{environment}";
    });

builder.Services.Configure<ServiceEndpointsConfiguration>(builder.Configuration.GetSection("ServiceEndpoints"));

// Add Ocelot with Polly and Swagger
builder.Services
       .AddOcelot(builder.Configuration)
       .AddPolly();

builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddOcelot(routes, builder.Environment)
    .AddEnvironmentVariables();

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

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Use Swagger for Ocelot and Add Ocelot Middleware
app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";

}).UseOcelot().Wait();

app.Run();