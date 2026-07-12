using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using RestaurantAPI;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Middleware;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using RestaurantAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var authenthicationSettings = new AuthenthicationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenthicationSettings);

builder.Services.AddAuthorization(options =>
{
    // dodajemy w³asn¹ politykê autoryzacji, która bêdzie wymaga³a posiadania claimu "Nationality" w token
    // dla tej polityki musi istnieæ claim Nationality aby spe³niæ wymagania - wystarczy, ¿e istnieje w tokenie, nie musi mieæ konkretnej wartoœci
    options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality")); // dowolna narowodoœæ

    // PRZYK£AD POLITYKI AUTORYZACJI, KTÓRA WYMAGA KONKRETNEJ WARTOŒCI CLAIMU - NP. WPUSZCZAMY TYLKO POLAKÓW :D
    // options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "Polish"));

    options.AddPolicy("AtLeast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
    // customowa polityka autoryzacji, która wymaga spe³nienia customowego wymagania MinimumAgeRequirement
    // definiujemy tutaj minimalny wiek, który musi spe³niaæ u¿ytkownik, aby uzyskaæ dostêp do endpointu, który bêdzie wymaga³ tej polityki
});

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
builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>(); // rejestracja customowego handlera autoryzacji, który bêdzie sprawdza³ czy u¿ytkownik spe³nia wymagania customowej polityki autoryzacji AtLeast20

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
