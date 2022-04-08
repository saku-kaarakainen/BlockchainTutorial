using DotNetRestApi;
using DotNetRestApi.Services;
using Microsoft.Extensions.Internal;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<Nodes>().AddHttpClient();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<Cryptograph>();
builder.Services.AddTransient<ConsensusMechanism>();
builder.Services.AddSingleton<Blockchain>();
builder.Services.AddSingleton<ISystemClock, SystemClock>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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