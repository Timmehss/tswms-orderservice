#region Usings

using Microsoft.EntityFrameworkCore;
using TSWMS.OrderService.Api.MappingProfiles;
using TSWMS.OrderService.Configurations;
using TSWMS.OrderService.Data;

#endregion

var builder = WebApplication.CreateBuilder(args);

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
    cfg.AddProfile<OrderMappingProfile>();
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

// Apply pending migrations to the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    dbContext.Database.Migrate();
}

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