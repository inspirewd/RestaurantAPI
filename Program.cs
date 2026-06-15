using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using RestaurantAPI;
using RestaurantAPI.Entities;
using RestaurantAPI.Middleware;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using RestaurantAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var authenthicationSettings = new AuthenthicationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenthicationSettings);

builder.Services.AddSingleton(authenthicationSettings); // potrzebne ¿eby wstrzykn¹æ do serwisu AccountService (jako singleton w kontenerze zale¿noœci), który bêdzie generowa³ tokeny JWT 
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg => 
{
    cfg.RequireHttpsMetadata = false; // nie wymuszamy od klienta korzystania z protoko³u https
    cfg.SaveToken = true;// dany token powinien zostaæ zapisany po stronie servera
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenthicationSettings.JwtIssuer, // wydawca danego tokenu
        ValidAudience = authenthicationSettings.JwtIssuer, // jakie podmioty mog¹ u¿ywaæ tego tokenu
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenthicationSettings.JwtKey)) // klucz prywatny
    };

});
// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddDbContext<RestaurantDbContext>();
builder.Services.AddScoped<RestaurantSeeder>();
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

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
app.UseMiddleware<ErrorHandlingMiddleware>(); // wa¿ne ¿eby to dodaæ przed UseHttpsRedirection - zapewnimy w³aœciwy przep³yw
app.UseMiddleware<RequestTimeMiddleware>();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
