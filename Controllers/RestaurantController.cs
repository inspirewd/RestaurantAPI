using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize] // w ten sposób włączamy autoryzację na całym kontrolerze, bez tego w ogóle ona nie działa - można też dodać tylko dla poszczególnych enpointów
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [Authorize(Policy = "HasNationality")]  // dodanie customowej autoryzacji, nazwa musi się pokrywać z tą, którą zdefiniowaliśmy w Program.cs w AddAuthorization
        public ActionResult<IEnumerable<RestaurantDto>> GetAll() 
        {
            return Ok(_restaurantService.GetAll());
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // w ten sposób wyłączamy autoryzację dla tego endpointu, mimo że jest włączona na całym kontrolerze
        public ActionResult<RestaurantDto> Get([FromRoute]int id)
        {
            var restaurant = _restaurantService.GetById(id);
            return Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")] // autoryzacja tylko dla ról Admin i Manager, autoryzacja na poziomie enpointa jest ważniejsza niż ta na całym kontrolerze u góry
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var id = _restaurantService.Create(dto);
            return Created($"/api/restaurant/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
        {
            _restaurantService.Update(id, dto);
            return Ok();
        }
    }
}
