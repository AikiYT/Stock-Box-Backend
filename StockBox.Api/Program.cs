using StockBox.Application.DependencyInjection;
using StockBox.Identity.DependencyInjection;
using StockBox.Identity.Seed;
using StockBox.Infrastructure.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:5173",
                    "https://systems-stock-box.94zvjo.easypanel.host" // Sin '/' al final
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
// 2. Controllers
builder.Services.AddControllers();

// 3. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Application
builder.Services.AddApplicationLayer();

// 5. Persistence
builder.Services.AddPersistenceInfrastructure(
    builder.Configuration);

// 6. Identity
builder.Services.AddIdentityInfrastructure(
    builder.Configuration);

var app = builder.Build();

// 7. Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "StockBox API v1");

    c.RoutePrefix = string.Empty;
});

// 8. CORS (Debe ir ANTES de HTTPS Redirection y Auth)
app.UseCors("AllowFrontend");

// 9. HTTPS
app.UseHttpsRedirection();

// 10. Authentication
app.UseAuthentication();

// 11. Authorization
app.UseAuthorization();

// 12. Controllers
app.MapControllers();

// 13. Identity Seeder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedAsync(services);
}

app.Run();