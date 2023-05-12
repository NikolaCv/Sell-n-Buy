using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SellnBuy.Api.Repositories;
using SellnBuy.Api.Repositories.InMemRepositories;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Settings;
using MongoDB.Driver;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.ComponentModel.DataAnnotations;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => 
{
	options.SuppressAsyncSuffixInActionNames = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRepository<User>, InMemUsersRepository>();
builder.Services.AddSingleton<IRepository<Advertisement>, InMemAdvertisementsRepository>();

builder.Services.AddScoped<IService<User, UserDto, CreateUserDto, UpdateUserDto>, UserService>();
builder.Services.AddScoped<IService<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>, AdvertisementService>();

//builder.Services.AddSingleton<IRepository<User>, >();
//builder.Services.AddSingleton<IRepository<Advertisement>, InMemAdvertisementsRepository>();

var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

builder.Services.AddSingleton<IMongoClient>(ServiceProvider => 
{
	return new MongoClient(mongoDbSettings?.ConnectionString);
});



var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        string errorMessage = exceptionHandlerPathFeature?.Error?.Message ?? "An unexpected error occurred.";

        switch (exceptionHandlerPathFeature?.Error)
        {
            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorMessage = "Not found";
                break;

            case ValidationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case Exception:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        await context.Response.WriteAsync(errorMessage);
    });
});


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
