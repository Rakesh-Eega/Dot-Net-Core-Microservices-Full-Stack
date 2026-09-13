using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Reflection;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

//Register custom serializer
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//register swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Register Mediatr
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(GetAllBrandsHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
//register services
builder.Services.AddScoped<IBrandRepository,BrandRepository>();
builder.Services.AddScoped<ITypeRepository,TypeRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();

//bind strongly typed settings
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DataBaseSettings"));

//Register MongoClient as Singleton
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings =sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new MongoClient(settings.connectionstring);
}
 );
var app = builder.Build();

//Seed Mongo Db on startUp
using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseSettings>>();
    await DatabaseSeeder.SeedAsync(config);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
//enable swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
