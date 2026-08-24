using StockBox.Application.DependencyInjection;
using StockBox.Identity.DependencyInjection;
using StockBox.Identity.Seed;
using StockBox.Infrastructure.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:5173",
                    "https://systems-stock-box.94zvjo.easypanel.host" // Sin '/' al final y sin rutas como '/products'
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationLayer();

builder.Services.AddPersistenceInfrastructure(
    builder.Configuration);

builder.Services.AddIdentityInfrastructure(
    builder.Configuration);

// 2. Prevenir que Identity haga redirección 302 a '/Account/Login' en API (Devuelve 401/403 limpio)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

var app = builder.Build();

// 3. Configurar Swagger para que cargue directamente en la raíz ("/")
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockBox API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

// 4. Activar CORS antes de la Autenticación/Autorización
app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedAsync(services);
}

app.Run();