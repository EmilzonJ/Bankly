using Application;
using Infrastructure;
using Web;
using Web.Config;

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

app.UseCors(StaticSetting.CorsPolicyName);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
