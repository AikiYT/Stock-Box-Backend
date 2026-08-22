using StockBox.Application.DependencyInjection;
using StockBox.Identity.DependencyInjection;
using StockBox.Identity.Seed;
using StockBox.Infrastructure.Persistence.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationLayer();

builder.Services.AddPersistenceInfrastructure(
    builder.Configuration);

builder.Services.AddIdentityInfrastructure(
    builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedAsync(services);
}

app.Run();