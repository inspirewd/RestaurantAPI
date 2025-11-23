using Microsoft.AspNetCore.Identity;

namespace RestaurantAPI.Entities
{
    public class User
    {
        public int Id { get; set; } // dzięki dodaniu Id tabela w bazi będzie automatycznie inkrementowana
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; } // referencje do id roli, która będzie kluczem obcym w tabeli z rolami
        public virtual Role Role { get; set; }
    }
}
