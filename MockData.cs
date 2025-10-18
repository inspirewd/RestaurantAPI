using RestaurantAPI.Entities;

public class MockData
{
    public List<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>
        {
            new Restaurant
            {
                Name = "La Bella Italia",
                Description = "Tradycyjna włoska restauracja z domowymi makaronami i pizzą.",
                Category = "Włoska",
                HasDelivery = true,
                ContactEmail = "contact@labellaitalia.com",
                ContactNumber = "+48 123 456 789",
                Address = new Address
                {
                    City = "Warszawa",
                    Street = "Marszałkowska 15",
                    PostalCode = "00-001"
                },
                Dishes = new List<Dish>
                {
                    new Dish
                    {
                        Name = "Pizza Margherita",
                        Description = "Klasyczna pizza z mozzarellą, pomidorami i bazylią.",
                        Price = 29.99m
                    },
                    new Dish
                    {
                        Name = "Spaghetti Carbonara",
                        Description = "Makaron spaghetti z sosem na bazie jajek, boczku i sera pecorino.",
                        Price = 34.50m
                    }
                }
            },
            new Restaurant
            {
                Name = "Sushi World",
                Description = "Autentyczne sushi przygotowywane przez japońskich mistrzów kuchni.",
                Category = "Japońska",
                HasDelivery = true,
                ContactEmail = "info@sushiworld.pl",
                ContactNumber = "+48 987 654 321",
                Address = new Address
                {
                    City = "Kraków",
                    Street = "Floriańska 22",
                    PostalCode = "31-019"
                },
                Dishes = new List<Dish>
                {
                    new Dish
                    {
                        Name = "Nigiri łosoś",
                        Description = "Ryż z plasterkiem świeżego łososia.",
                        Price = 12.00m
                    },
                    new Dish
                    {
                        Name = "California Roll",
                        Description = "Rolki z krabem, awokado i ogórkiem.",
                        Price = 24.00m
                    }
                }
            },
            new Restaurant
            {
                Name = "Burger House",
                Description = "Amerykańskie burgery z najlepszej jakości wołowiny.",
                Category = "Amerykańska",
                HasDelivery = false,
                ContactEmail = "hello@burgerhouse.pl",
                ContactNumber = "+48 555 222 111",
                Address = new Address
                {
                    City = "Gdańsk",
                    Street = "Długa 10",
                    PostalCode = "80-831"
                },
                Dishes = new List<Dish>
                {
                    new Dish
                    {
                        Name = "Classic Burger",
                        Description = "Wołowina, ser cheddar, sałata, pomidor i sos burgerowy.",
                        Price = 27.00m
                    },
                    new Dish
                    {
                        Name = "BBQ Bacon Burger",
                        Description = "Wołowina, boczek, ser, cebula i sos BBQ.",
                        Price = 32.00m
                    }
                }
            }
        };
    }
}
