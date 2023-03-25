using Microsoft.AspNetCore.Mvc;
using ReactTest.Models;
using System.Formats.Asn1;
using System.Globalization;
using System;
using Microsoft.AspNetCore.Hosting.Server;
using System.Text;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using ReactTest.Utils;

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
            //var belarusRecordsGenerator = new BelarusUserDataGenerator("E:\\programming\\Itransition\\ReactTest\\data\\belarus");
            //var polandUserDataGenerator = new PolandUserDataGenerator("E:\\programming\\Itransition\\ReactTest\\data\\Poland")
            var usaUserDataGenerator = new USAUserDataGenerator("E:\\programming\\Itransition\\ReactTest\\data\\USA");

            int howMuchGenerate = lengthGeneratedPrev == 0 ? 10 : 5;
            var resultingRecords = new List<UserDataModel>();
            int currentRandomSeed = randomSeed;
            bool isRegenerateSeed = false;
            if (lengthGeneratedPrev>0)
            {
                isRegenerateSeed = true;
            }
            int previousRandomSeed = currentRandomSeed;
            for (int newRecordNumber = 0; newRecordNumber < howMuchGenerate; newRecordNumber++)
            {
                UserDataModel resultRecord = usaUserDataGenerator.GenerateRecord(errorsPerRecord, previousRandomSeed ,isRegenerateSeed, out currentRandomSeed);
                resultRecord.number = newRecordNumber + lengthGeneratedPrev + 1;
                previousRandomSeed = currentRandomSeed;//for repeatability
                resultingRecords.Add(resultRecord);
                isRegenerateSeed = false;
            }

            return resultingRecords;
        }

        [HttpGet]
        public IEnumerable<string> GetRegions()
        {
            var regions = new List<string>() { "USA","Belarus", "Poland"};
            return regions;
        }
    }
}