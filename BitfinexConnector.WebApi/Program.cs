using BitfinexConnector.Connector;
using BitfinexConnector.Infrastructure.Interfaces;
using BitfinexConnector.Infrastructure.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ConnectorOptions>(builder.Configuration.GetSection("ConnectorOptions"));
builder.Services.AddSingleton<IConnectorWebApi, ConnectorWebApi>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
