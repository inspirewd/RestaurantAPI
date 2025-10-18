using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll() 
        {
            return Ok(_restaurantService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute]int id)
        {
            var restaurant = _restaurantService.GetById(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            var id = _restaurantService.Create(dto);
            return Created($"/api/restaurant/{id}", null);
        }
    }
}
