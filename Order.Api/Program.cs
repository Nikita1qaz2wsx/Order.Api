using Order.Api.Endpoints;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.MapEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<Order.Api.Middleware.CorrelationIdMiddleware>();
app.UseMiddleware<Order.Api.Middleware.GlobalExceptionMiddleware>();
app.UseMiddleware<Order.Api.Middleware.RequestLoggingMiddleware>();

app.Run();