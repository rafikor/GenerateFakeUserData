using Microsoft.AspNetCore.Mvc;

namespace ReactTest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<WeatherForecast> GetForecast([FromHeader] string title, [FromHeader] int lengthGeneratedPrev)
        {
            int howMuchGenerate = lengthGeneratedPrev == 0 ? 10 : 5;
            return Enumerable.Range(1, howMuchGenerate).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)] + title+ lengthGeneratedPrev.ToString()
            })
            .ToArray();
        }

        [HttpGet]
        public IEnumerable<string> GetRegions()
        {
            var regions = new List<string>() { "USA","Belarus", "Georgia"};
            return regions;
        }
    }
}