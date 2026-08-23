using StockBox.Application.DependencyInjection;
using StockBox.Identity.DependencyInjection;
using StockBox.Identity.Seed;
using StockBox.Infrastructure.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// --- AGREGAR CORS AQUÍ ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173") // Cambia las URLs por las de tu Frontend (React, Angular, Vue, etc.)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Necesario si manejas cookies o autenticación basada en tokens/sesión
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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// --- ACTIVAR CORS AQUÍ (IMPORTANTE: Debe ir ANTES de UseAuthentication y UseAuthorization) ---
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