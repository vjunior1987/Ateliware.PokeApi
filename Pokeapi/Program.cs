using Pokeapi.Services;
using Pokeapi.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Using dependency injection for both service and wrapper for httpClient. the client was added as transient in order to have its lifecycle end after executing the service method.
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();

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
