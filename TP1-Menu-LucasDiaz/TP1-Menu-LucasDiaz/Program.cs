using Applications.Interface;
using Applications.Interface.Category;
using Applications.Interface.Category.ICategoryService;
using Applications.Interface.DeliveryType;
using Applications.Interface.DeliveryType.IDeliveryTypeService;
using Applications.Interface.Dish;
using Applications.Interface.Dish.IDishService;
using Applications.Interface.IOrderItem;
using Applications.Interface.Order;
using Applications.Interface.Order.IOrder;
using Applications.Interface.Status;
using Applications.Interface.Status.IStatusService;
using Applications.UseCase;
using Applications.UseCase.CategoryService;
using Applications.UseCase.DeliveryType;
using Applications.UseCase.DishService;
using Applications.UseCase.Order;
using Applications.UseCase.Status;
using Infrastructure.Command;
using Infrastructure.Data;
using Infrastructure.Query;
using Microsoft.EntityFrameworkCore;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurar EF Core con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//INJECTIONS
//builder Dish
builder.Services.AddScoped<IDishCommand, DishCommand>();
builder.Services.AddScoped<IDishQuery, DishQuery>();
//builder DishUseCase
builder.Services.AddScoped<IDishCreate, DishCreate>();
builder.Services.AddScoped<IDishUpdate, DishUpdate>();
builder.Services.AddScoped<IDishGetAllAsync, DishGetAllAsync>();
builder.Services.AddScoped<IDishExistId, DishExistId>();
builder.Services.AddScoped<IDishExistName, DishExistName>();
builder.Services.AddScoped<IDishDelete, DishDelete>();
builder.Services.AddScoped<IDishGetById, DishGetById>();
//builder.Services.AddScoped<IDishCategory, DishCategory>();

//builder Query
builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();
builder.Services.AddScoped<ICategoryCommand, CategoryCommand>();
//builder CategoryUseCase
builder.Services.AddScoped<ICategoryGetById, CategoryGetById>();
builder.Services.AddScoped<ICategoryExist, CategoryExist>();
builder.Services.AddScoped<ICategoryGetAll, CategoryGetAll>();

//builder Order
builder.Services.AddScoped<IOrderCommand, OrderCommand>();
builder.Services.AddScoped<IOrderQuery, OrderQuery>();
//builder OrderUseCase
builder.Services.AddScoped<IOrderCreate, OrderCreate>();
builder.Services.AddScoped<IOrderGetById, OrderGetById>();
builder.Services.AddScoped<IOrderGetAllAsync, OrderGetAllAsync>();
builder.Services.AddScoped<IUpdateItemFromOrder, UpdateItemFromOrder>();
builder.Services.AddScoped<IUpdateOrderItemStatus, UpdateOrderItemStatus>();


//builder DeliveryType
builder.Services.AddScoped<IDeliveryTypeQuery, DeliveryTypeQuery>();
builder.Services.AddScoped<IDeliveryTypeGetAll, DeliveryTypeGetAll>();

//builder OrderItem
builder.Services.AddScoped<IOrderItemCommand, OrderItemCommand>();
builder.Services.AddScoped<IOrderItemQuery, OrderItemQuery>();

//builder status
builder.Services.AddScoped<IStatusQuery, StatusQuery>();
builder.Services.AddScoped<IStatusGetAll, StatusGetAll>();


//
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TP1-Menu-LucasDiaz",
        Version = "v1",
        Description = "API del proyecto TP1-Menu-LucasDiaz"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    //c.EnableAnnotations();
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

//// Middleware custom for exception handling
//app.UseMiddleware<ErrorHandlingMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
