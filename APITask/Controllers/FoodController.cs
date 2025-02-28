using APITask.Implimention;
using APITask.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {


        private readonly IIFoodService   _foodService;
            
        public FoodController(IIFoodService foodService)
        {
            _foodService= foodService;
        }

        [HttpGet]
        public IActionResult GetFoods()
        {
            var foods = _foodService.GetAvailableFoods();
            return Ok(foods);
        }
    }

}
