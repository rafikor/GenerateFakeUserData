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
            var belarusRecordsGenerator = new BelarusUserDataGenerator("E:\\programming\\Itransition\\ReactTest\\data\\belarus");

            int howMuchGenerate = lengthGeneratedPrev == 0 ? 10 : 5;
            var resultingRecords = new List<UserDataModel>();
            int currentRandomSeed = randomSeed;
            int previousRandomSeed = randomSeed;
            for (int newRecordNumber = 0; newRecordNumber < howMuchGenerate; newRecordNumber++)
            {
                UserDataModel resultRecord = belarusRecordsGenerator.GenerateRecord(errorsPerRecord, previousRandomSeed, out currentRandomSeed);
                previousRandomSeed = currentRandomSeed;
                resultRecord.number = newRecordNumber + lengthGeneratedPrev;
                resultingRecords.Add(resultRecord);
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