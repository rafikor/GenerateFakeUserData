using Microsoft.AspNetCore.Mvc;
using ReactTest.Models;

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
        public IEnumerable<UserDataModel> GetForecast([FromHeader] string selectedRegion, 
            [FromHeader] int lengthGeneratedPrev, [FromHeader] double errorsPerRecord, [FromHeader] int randomSeed)
        {

            int howMuchGenerate = lengthGeneratedPrev == 0 ? 10 : 5;
            return Enumerable.Range(1, howMuchGenerate).Select(index => new UserDataModel
            {
                number = index+ lengthGeneratedPrev,
                randomId = Convert.ToString(5),
                fullName = selectedRegion+"name",
                adress = selectedRegion + "adress",
                phone = selectedRegion + "phone"
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