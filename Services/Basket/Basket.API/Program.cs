using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infracture.Repositories;
using Basket.Infracture.Settings;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();
//add Application Services
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register mediatR services
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(CreateShoppingCartHandler).Assembly
};
builder.Services.AddMediatR(cfg =>cfg.RegisterServicesFromAssemblies(assemblies));

//options Pattern
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
//Redis
builder.Services.AddStackExchangeRedisCache((cfg) =>
{
    cfg.Configuration = builder.Configuration.GetSection("CacheSettings:ConnectionString").Value;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
