#region Usings



#endregion

using TSWMS.ProductService.Api.MappingProfiles;
using TSWMS.ProductService.Configurations;

var builder = WebApplication.CreateBuilder(args);

var developmentEnvironments = new string[] { "Development", "Production" };

// Get Environment
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Configure App Configuration
builder.Configuration
    .AddJsonFile($"appsettings.{environment}.json");

// Add Cors Policie
builder.Services.AddCors(o => o.AddPolicy("TSWMSPolicy", builder =>
{
    builder.SetIsOriginAllowed((host) => true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
}));

// Configure AutoMapper Profiles
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ProductMappingProfile>();
});

// Configure EntityFramework UserDbContext
builder.Services.ConfigureUserDbContext(builder.Configuration);

// Configure dependency injection for managers
builder.Services.ConfigureManagers();

// Configure dependency injection for repositories
builder.Services.ConfigureRepositories();

// Additional service registrations
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("TSWMSPolicy");

// Configure request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();