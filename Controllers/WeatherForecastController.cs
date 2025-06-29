using Microsoft.AspNetCore.Mvc;
using MyApiTest.Models;
using MyApiTest.Services;

namespace MyApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastService _service;

        public WeatherForecastController(WeatherForecastService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WeatherForecast>> Get()
        {
            return Ok(_service.GetForecast());
        }
    }
}
