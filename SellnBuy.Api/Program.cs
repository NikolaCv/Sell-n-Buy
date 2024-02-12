using SellnBuy.Api.Repositories;
using SellnBuy.Api.Entities;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.ComponentModel.DataAnnotations;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Services;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using SellnBuy.Api.Data;
using Microsoft.EntityFrameworkCore;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Entities.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
	options.SuppressAsyncSuffixInActionNames = false;
});

// For MongoDb
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// InMemRepositories
// builder.Services.AddSingleton<IRepository<User>, InMemUserRepository>();
// builder.Services.AddSingleton<IRepository<Advertisement>, InMemAdvertisementRepository>();

builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(AdvertisementMappingProfile));

var connectionString = builder.Configuration.GetConnectionString("SellnBuyContext");
builder.Services.AddSqlServer<SellnBuyContext>(connectionString);

builder.Services.AddScoped<IRepository<User>, EntityFrameworkUsersRepository>();
builder.Services.AddScoped<IRepository<Advertisement>, EntityFrameworkAdvertisementsRepository>();
builder.Services.AddScoped<IRepository<Condition>, EntityFrameworkConditionsRepository>();
builder.Services.AddScoped<IRepository<Category>, EntityFrameworkCategoriesRepository>();

builder.Services.AddScoped<IService<User, UserDto, CreateUserDto, UpdateUserDto>, UsersService>();
builder.Services.AddScoped<IService<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>, AdvertisementsService>();
builder.Services.AddScoped<IService<Condition, ConditionDto, CreateConditionDto, UpdateConditionDto>, ConditionsService>();
builder.Services.AddScoped<IService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto>, CategoriesService>();

var app = builder.Build();

app.Services.InitializeDb();

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
				errorMessage = "The item with requested ID does not exist.";
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
