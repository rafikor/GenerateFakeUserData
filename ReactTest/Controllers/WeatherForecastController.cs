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
        private static Dictionary<string,BaseUserDataGenerator> dataGenerators;

        private readonly ILogger<WeatherForecastController> _logger;

        static WeatherForecastController()
        {
            dataGenerators = new Dictionary<string,BaseUserDataGenerator>();
            dataGenerators.Add("USA", new USAUserDataGenerator("E:\\programming\\Itransition\\ReactTest\\data\\USA"));
            dataGenerators.Add("Belarus", new BelarusUserDataGenerator("E:\\programming\\Itransition\\ReactTest\\data\\Belarus"));
            dataGenerators.Add("Poland", new PolandUserDataGenerator("E:\\programming\\Itransition\\ReactTest\\data\\Poland"));
        }

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

       

        [HttpPost]
        public IEnumerable<UserDataModel> GetForecast([FromHeader] string selectedRegion,
            [FromHeader] int lengthGeneratedPrev, [FromHeader] double errorsPerRecord, [FromHeader] int randomSeed)
        {
            BaseUserDataGenerator dataGenerator = dataGenerators[selectedRegion];

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
                UserDataModel resultRecord = dataGenerator.GenerateRecord(errorsPerRecord, previousRandomSeed ,isRegenerateSeed, out currentRandomSeed);
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
            return dataGenerators.Keys;
        }

        [HttpGet]
        public FileResult DownloadCsv()
        {
            string fileName = "E:\\programming\\Itransition\\ReactTest\\data\\USA\\majorCities.csv";
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);

            return File(fileBytes, "text/csv", "result.csv"); // this is the key!
        }
    }
}