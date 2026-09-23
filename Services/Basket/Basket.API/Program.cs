using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Application.Settings;
using Basket.Core.Repositories;
using Basket.Infracture.Repositories;
using Basket.Infracture.Settings;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Options;
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
builder.Services.Configure<GrpcSettings>(builder.Configuration.GetSection("GrpcSettings"));
//Redis
builder.Services.AddStackExchangeRedisCache((cfg) =>
{
    cfg.Configuration = builder.Configuration.GetSection("CacheSettings:ConnectionString").Value;
});
//Grpc
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    (sp, config) => {
        var grpcSettings = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
        config.Address = new Uri(grpcSettings.DiscountUrl);
    }
    );
builder.Services.AddScoped<DiscountGrpcService>();
//builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>((sp, config) => {
//    var grpcSettings = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
//    config.Address = new Uri(grpcSettings.DiscountUrl);
//});
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
