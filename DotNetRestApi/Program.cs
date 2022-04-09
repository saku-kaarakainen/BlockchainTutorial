using DotNetRestApi;
using DotNetRestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Internal;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;

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
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options => 
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(options => 
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    // Needs to be included from the project properties
    //string xmlDocDirectory = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    //swaggerGenOptions.IncludeXmlComments(xmlDocDirectory);

    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "My Blockchain REST API v1",
        Description = "My attempt to make my first blockchain network",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Saku Kaarakainen",
            Email = "sakuba91@hotmail.com",
            Url = new Uri("https://github.com/saku-kaarakainen")
        }
    });
    swaggerGenOptions.UseAllOfToExtendReferenceSchemas();
});
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    // We expose swagger, because this is a demo app.
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();