using Application;
using Infrastructure;
using Infrastructure.MongoContext;
using Web;
using Web.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(StaticSettings.CorsPolicyName);

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

await SeedDataAsync(app);

app.Run();

return;

async Task SeedDataAsync(WebApplication application)
{
    using var scope = application.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var mongoDbContext = services.GetRequiredService<MongoDbContext>();
        mongoDbContext.CreateIndexes();
        await mongoDbContext.SeedCollectionsAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
