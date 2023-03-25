using Microsoft.AspNetCore.Mvc;
using GenerateFakeUserData.Models;
using System.Formats.Asn1;
using System.Globalization;
using System;
using Microsoft.AspNetCore.Hosting.Server;
using System.Text;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using GenerateFakeUserData.Utils;
using Microsoft.AspNetCore.WebUtilities;
using ServiceStack.Text;
using ServiceStack;

namespace GenerateFakeUserData.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GenerateFakeUserController : ControllerBase
    {
        private static Dictionary<string,BaseUserDataGenerator> dataGenerators;

        private readonly ILogger<GenerateFakeUserController> _logger;

        static GenerateFakeUserController()
        {
            dataGenerators = new Dictionary<string,BaseUserDataGenerator>();
            dataGenerators.Add("USA", new USAUserDataGenerator(Path.Combine("data","USA")));
            dataGenerators.Add("Belarus", new BelarusUserDataGenerator(Path.Combine("data", "Belarus")));
            dataGenerators.Add("Poland", new PolandUserDataGenerator(Path.Combine("data", "Poland")));
        }

        public GenerateFakeUserController(ILogger<GenerateFakeUserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<UserDataModel> GetNewData([FromHeader] string selectedRegion,
            [FromHeader] int lengthGeneratedPrev, [FromHeader] double errorsPerRecord, [FromHeader] int randomSeed)
        {
            int howMuchGenerate = lengthGeneratedPrev == 0 ? 10 : 5;
            return GenerateRecords(selectedRegion,
            lengthGeneratedPrev, errorsPerRecord, randomSeed, howMuchGenerate);
        }

        private IEnumerable<UserDataModel>  GenerateRecords(string selectedRegion,
            int lengthGeneratedPrev, double errorsPerRecord, int randomSeed, int countToGenerate)
        {
            BaseUserDataGenerator dataGenerator = dataGenerators[selectedRegion];
            
            var resultingRecords = new List<UserDataModel>();
            int currentRandomSeed = randomSeed;
            bool isRegenerateSeed = false;
            if (lengthGeneratedPrev > 0)
            {
                isRegenerateSeed = true;
            }
            int previousRandomSeed = currentRandomSeed;
            for (int newRecordNumber = 0; newRecordNumber < countToGenerate; newRecordNumber++)
            {
                UserDataModel resultRecord = dataGenerator.GenerateRecord(errorsPerRecord, previousRandomSeed, isRegenerateSeed, out currentRandomSeed);
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

        [HttpPost]
        public FileResult DownloadCsv([FromHeader] string selectedRegion,
            [FromHeader] int lengthToGenerate, [FromHeader] double errorsPerRecord, [FromHeader] int randomSeed)
        {
            var records = GenerateRecords(selectedRegion,0, errorsPerRecord, randomSeed, lengthToGenerate);
            var result = CsvSerializer.SerializeToCsv(records);
            byte[] fileBytes = result.ToUtf8Bytes();

            return File(fileBytes, "text/csv", "result.csv");
        }
    }
}