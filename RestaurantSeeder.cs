using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly MockData _mock;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
            _mock = new MockData();
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect()) // sprawdzenie polaczenia z baza danych
            {

                if (!_dbContext.Roles.Any()) 
                {
                    var roles = _mock.GetRoles(); // seeder do zasilenia przykładowych, zamockowanych danych wywoła się przy starcie aplikacji
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                } 
                if (!_dbContext.Restaurants.Any()) // sprawdzenie czy tabela restaurant jest pusta
                {
                    var restaurants = _mock.GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges(); // musimy zapisać zmiany na kontekście bazy danych po dodaniu mocka
                }
            }
        }
    }
}
