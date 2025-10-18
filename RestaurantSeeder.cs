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
