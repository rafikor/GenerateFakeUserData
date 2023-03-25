using Microsoft.AspNetCore.Mvc;
using ReactTest.Models;
using System.Formats.Asn1;
using System.Globalization;
using System;
using Microsoft.AspNetCore.Hosting.Server;
using System.Text;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;

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

        protected List<string> LoadSimpleOneRowFileData(string filePath)
        {
            var csvRows = System.IO.File.ReadAllLines(filePath, Encoding.UTF8).ToList();
            var result = new List<string>();
            foreach (var row in csvRows)
            {
                var rowTrim = row.Trim();
                if (rowTrim!="")
                    result.Add(row);
            }
            return result;
        }

        protected string getRandomElement(List<string> array, Random random)
        {
            var result = array[random.Next(array.Count)];
            return result;
        }

        [HttpPost]
        public IEnumerable<UserDataModel> GetForecast([FromHeader] string selectedRegion,
            [FromHeader] int lengthGeneratedPrev, [FromHeader] double errorsPerRecord, [FromHeader] int randomSeed)
        {

            var path = "E:\\programming\\Itransition\\ReactTest\\data\\belarus\\cities.csv";
            var csvRows = System.IO.File.ReadAllLines(path, Encoding.UTF8).ToList();
            var cities = new List<string>();
            foreach (var row in csvRows)
            {
                cities.Add(row);
            }

            var pathVillages = "E:\\programming\\Itransition\\ReactTest\\data\\belarus\\villages.csv";
            var csvRowsVillages = System.IO.File.ReadAllLines(pathVillages, Encoding.UTF8).ToList();
            var villages = new List<string>();
            foreach (var row in csvRowsVillages)
            {
                villages.Add(row);
            }

            /*var pathAbbreviations = "E:\\programming\\Itransition\\ReactTest\\data\\belarus\\abberviationsLivePlaceTypes.csv";
            var csvRowsAbbreviations = System.IO.File.ReadAllLines(pathAbbreviations, Encoding.UTF8).ToList();
            var abbreviations = new Dictionary<string,string>();
            foreach (var row in csvRows)
            {
                var columns = row.Split(';');

                var initial = columns[0];
                var abbreviated = columns[1];
                abbreviations[initial] = abbreviated;
            }*/

            var pathRegions = "E:\\programming\\Itransition\\ReactTest\\data\\belarus\\regions.csv";
            var csvRowsRegions = System.IO.File.ReadAllLines(pathRegions, Encoding.UTF8).ToList();
            var regions = new List<string>();
            foreach (var row in csvRows)
            {
                regions.Add(row);
            }

            var pathStreets = "E:\\programming\\Itransition\\ReactTest\\data\\belarus\\streets.csv";
            var csvRowsStreets = System.IO.File.ReadAllLines(pathStreets, Encoding.UTF8).ToList();
            var streets = new List<string>();
            foreach (var row in csvRowsStreets)
            {
                streets.Add(row);
            }

            var pathVillageSovets = "E:\\programming\\Itransition\\ReactTest\\data\\belarus\\villageSoviets.csv";
            var csvRowsVillageSovets = System.IO.File.ReadAllLines(pathVillageSovets, Encoding.UTF8).ToList();
            var villageSovets = new List<string>();
            foreach (var row in csvRowsVillageSovets)
            {
                villageSovets.Add(row);
            }


            var majorSities = new List<string>() { "г. Мінск", "г. Гомель", "г. Брэст", "г. Гродна", "г. Віцебск", "г. Магілеў" };
            var areas = new List<string>() { "Мінская вобл.", "Гомельская  вобл.", "Брэстская  вобл.", "Гродзенская  вобл.", "Віцебская  вобл.", "Магілеўская  вобл.","" };

            var possibleCountryNames = new List<string>() { "Беларусь", "Республіка Беларусь", "РБ","" };

            List<string> maleNames = LoadSimpleOneRowFileData("E:\\programming\\Itransition\\ReactTest\\data\\belarus\\male names.txt");
            List<string> maleSurnames = LoadSimpleOneRowFileData("E:\\programming\\Itransition\\ReactTest\\data\\belarus\\male surnames.txt");
            List<string> malePatronymics = LoadSimpleOneRowFileData("E:\\programming\\Itransition\\ReactTest\\data\\belarus\\male patronymics.txt");

            List<string> femaleNames = LoadSimpleOneRowFileData("E:\\programming\\Itransition\\ReactTest\\data\\belarus\\female names.txt");
            List<string> femaleSurnames = LoadSimpleOneRowFileData("E:\\programming\\Itransition\\ReactTest\\data\\belarus\\female surnames.txt");
            List<string> femalePatronymics = LoadSimpleOneRowFileData("E:\\programming\\Itransition\\ReactTest\\data\\belarus\\female patronymics.txt");
            string alphabet = "АБВГДЕЁЖЗІЙКЛМНОПРСТУЎФХЦЧШЫЬЭЮЯабвгдеёжзійклмнопрстуўфхцчшыьэюя";


            var phoneCodesBeginningNumbers = new List<string> { "29", "44", "33", "17", "15", "22" };
            var phoneCodesBeginningFormats = new List<string> { "8 0x ", "80x", "+375 (x) ", "+357-x-" };

            var phoneEndFormats = new List<string> { "xxx-xx-xx", "xxx xx xx", "xxxxxxx" };

            int howMuchGenerate = lengthGeneratedPrev == 0 ? 10 : 5;
            var resultingRecords = new List<UserDataModel>();
            int currentRandomSeed = randomSeed;
            int previousRandomSeed = randomSeed;
            for (int newRecordNumber = 0; newRecordNumber < howMuchGenerate; newRecordNumber++)
            {
                var random = new Random(currentRandomSeed);
                currentRandomSeed = random.Next();

                var countryName = possibleCountryNames[random.Next(possibleCountryNames.Count)];

                var area = "";
                var villageSoviet = "";
                var livingPlace = "";
                var street = streets[random.Next(streets.Count)];
                var part = "";
                var building = "";
                var room = "";


                var whatToGenerate = random.Next(2);
                switch (whatToGenerate)
                {
                    case 0:
                        livingPlace = majorSities[random.Next(majorSities.Count)];
                        break;
                    default:
                        var whatLivingPlaceType = random.Next(2);
                        bool isCity = whatLivingPlaceType == 1;
                        area = areas[random.Next(areas.Count)];
                        if (isCity)
                        {
                            livingPlace = cities[random.Next(cities.Count)];
                        }
                        else
                        {
                            livingPlace = villages[random.Next(villages.Count)];
                            villageSoviet = villageSovets[random.Next(villageSovets.Count)];
                        }
                        break;
                }
                building = "б. " + random.Next(1, 101).ToString();
                part = random.Next(3) == 0 ? "" : "к. " + random.Next(1, 4).ToString();

                room = part != "" || random.Next(2) == 0 ? "кв. " + random.Next(1, 81).ToString() : "";

                var indexRange = (210000, 247999);
                var index = random.Next(indexRange.Item1, indexRange.Item2).ToString();

                List<string> adress;
                var adressFormat = random.Next(3);
                if (adressFormat == 0)
                {
                    adress = new List<string> { index, countryName, area, villageSoviet, livingPlace, street, building, part, room };
                }
                else
                {
                    if (adressFormat == 2)
                    {
                        adress = new List<string> { countryName, area, villageSoviet, livingPlace, street, building, part, room, index };
                    }
                    else
                    {
                        adress = new List<string> { room, part, building, street, livingPlace, villageSoviet, area, countryName, index };
                    }
                }

                var stringAdress = "";
                foreach (var adresspart in adress)
                {
                    if (adresspart.Length > 0)
                    {
                        stringAdress += adresspart + ", ";
                    }
                }
                stringAdress = stringAdress.Substring(0, stringAdress.Length - 2);




                var phoneCodesBeginningNumber = phoneCodesBeginningNumbers[random.Next(phoneCodesBeginningNumbers.Count)];
                var phoneCodesBeginningFormat = phoneCodesBeginningFormats[random.Next(phoneCodesBeginningFormats.Count)];
                var phoneCodesBeginning = phoneCodesBeginningFormat.Replace("x", phoneCodesBeginningNumber);
                var phoneEndFormat = phoneEndFormats[random.Next(phoneEndFormats.Count)];
                var phoneEnd = "";
                for (var i = 0; i < phoneEndFormat.Length; i++)
                {
                    var letter = phoneEndFormat[i];
                    var minNumber = i == 0 ? 1 : 0;

                    if (letter == 'x')
                    {
                        letter = random.Next(minNumber, 10).ToString()[0];
                    }
                    phoneEnd += letter;
                }
                var phoneNumber = phoneCodesBeginning + phoneEnd;

                var fullName = "";



                bool isMale = random.Next(2) == 0;
                if (isMale)
                {
                    fullName = getRandomElement(maleSurnames, random) + " " + getRandomElement(maleNames, random) +
                        " " + getRandomElement(malePatronymics, random);
                }
                else
                {
                    fullName = getRandomElement(femaleSurnames, random) + " " + getRandomElement(femaleNames, random) +
                        " " + getRandomElement(femalePatronymics, random);
                }

                int intPartOfErrorsPerRecord = (int)errorsPerRecord;
                double floatPartOfErrorsPerRecord = errorsPerRecord - intPartOfErrorsPerRecord;
                int intErrorsPerRecord = intPartOfErrorsPerRecord;
                var rnd01 = random.NextDouble();
                if (rnd01 < floatPartOfErrorsPerRecord)
                {
                    intErrorsPerRecord += 1;
                }

                //string fullRecord = fullName + '@' + stringAdress + '@' + phoneNumber;
                var changedData = new string[] { fullName, stringAdress, phoneNumber };
                for (int i = 0; i < intErrorsPerRecord; i++)
                {
                    int errorLocation = random.Next(3);

                    int errorType = random.Next(3);
                    int errorPosition = random.Next(changedData[errorLocation].Length);
                    switch (errorType)
                    {
                        case 0://remove
                            var result = changedData[errorLocation].Substring(0, errorPosition);
                            if (errorPosition + 1 < changedData[errorLocation].Length)
                            {
                                result = result + changedData[errorLocation].Substring(errorPosition + 1);
                            }
                            break;
                        case 1://add
                            var symbolToAdd = errorLocation == 2 ? random.Next(10).ToString() : alphabet[random.Next(alphabet.Length)].ToString();
                            changedData[errorLocation] = changedData[errorLocation].Insert(errorPosition, symbolToAdd);
                            //changedData[errorLocation] = changedData[errorLocation].Substring(0, errorPosition) +
                            //    symbolToAdd + changedData[errorLocation].Substring(errorPosition);
                            break;
                        default://swap
                            if (changedData[errorLocation].Length > 1)
                            {
                                if (errorPosition != changedData[errorLocation].Length - 1)
                                {
                                    var symbolFirst = changedData[errorLocation][errorPosition];
                                    var symbolSecond = changedData[errorLocation][errorPosition + 1];
                                    changedData[errorLocation] = changedData[errorLocation].Substring(0, errorPosition) + symbolSecond + symbolFirst + changedData[errorLocation].Substring(errorPosition + 2);
                                }
                            }
                            break;
                    }
                }

                //var splittedResult = fullRecord.Split('@');
                fullName = changedData[0];
                stringAdress = changedData[1];
                phoneNumber = changedData[2];

                var resultRecord = new UserDataModel()
                {
                    number = newRecordNumber + lengthGeneratedPrev,
                    randomId = Convert.ToString(previousRandomSeed),
                    fullName = fullName,
                    adress = stringAdress,
                    phone = phoneNumber
                };
                previousRandomSeed = currentRandomSeed;
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