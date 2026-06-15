namespace RestaurantAPI
{
    public class AuthenthicationSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireDays { get; set; }

    }
}
