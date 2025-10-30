using NLog.Web;
using RestaurantAPI;
using RestaurantAPI.Entities;
using RestaurantAPI.Middleware;
using RestaurantAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<RestaurantDbContext>();
builder.Services.AddScoped<RestaurantSeeder>();
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.

// SEEDING bazy danych
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
    seeder.Seed();
}
app.UseMiddleware<ErrorHandlingMiddleware>(); // wa�ne �eby to doda� przed UseHttpsRedirection - zapewnimy w�a�ciwy przep�yw
app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
