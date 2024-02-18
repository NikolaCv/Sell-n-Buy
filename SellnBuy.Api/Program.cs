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
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddControllers(options =>
				{
					options.SuppressAsyncSuffixInActionNames = false;
				})
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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
builder.Services.AddAutoMapper(typeof(ConditionMappingProfile));
builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));

var connectionString = builder.Configuration.GetConnectionString("SellnBuyContext");
builder.Services.AddSqlServer<SellnBuyContext>(connectionString);
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	// Configure identity options here
	options.User.RequireUniqueEmail = true;

	options.Password.RequiredLength = 8;
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<SellnBuyContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IUsersRepository, EntityFrameworkUsersRepository>();
builder.Services.AddScoped<IBaseRepository<Advertisement>, EntityFrameworkAdvertisementsRepository>();
builder.Services.AddScoped<IBaseRepository<Condition>, EntityFrameworkConditionsRepository>();
builder.Services.AddScoped<IBaseRepository<Category>, EntityFrameworkCategoriesRepository>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBaseService<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>, AdvertisementsService>();
builder.Services.AddScoped<IBaseService<Condition, ConditionDto, CreateConditionDto, UpdateConditionDto>, ConditionsService>();
builder.Services.AddScoped<IBaseService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto>, CategoriesService>();

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
				break;

			case ValidationException:
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				errorMessage = "Bad Request!@";
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
