using System;
using System.CodeDom.Compiler;
using System.Text;

namespace GenerateFakeUserData.Utils
{
    public class BelarusUserDataGenerator : BaseUserDataGenerator
    {
        protected List<string> regions;
        protected List<string> villageSovets;
        protected List<string> areas;
        protected List<string> phoneCodesBeginningNumbers;
        public BelarusUserDataGenerator(string dataFolder):base(dataFolder) 
        {
            regions = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "regions.csv"));
            villageSovets = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "villageSoviets.csv"));
            areas = new List<string>() { "Мінская вобл.", "Гомельская  вобл.", "Брэстская  вобл.", "Гродзенская  вобл.", "Віцебская  вобл.", "Магілеўская  вобл.", "" };

            alphabet = "АБВГДЕЁЖЗІЙКЛМНОПРСТУЎФХЦЧШЫЬЭЮЯабвгдеёжзійклмнопрстуўфхцчшыьэюя";

            phoneCodesBeginningNumbers = new List<string> { "29", "44", "33", "17", "15", "22" };
            phoneCodesBeginningFormats = new List<string> { "8 0x ", "80x", "+375 (x) ", "+357-x-" };

            phoneEndFormats = new List<string> { "xxx-xx-xx", "xxx xx xx", "xxxxxxx" };
            postIndexRange = (210000, 247999);
        }
        
        protected override StringBuilder finishAdressGeneration(string index, string countryName, string livingPlace, TypeOfLivingPlace whatLivingPlaceType, string street, string buildingNumber, string roomNumber)
        {
            var building = "б. " + buildingNumber;

            int maxBuildingPartNumber = 3;
            int optionsToCorrectProbability = 3;
            var part = random.Next(optionsToCorrectProbability) == 0 ? "" : "к. " + random.Next(1, maxBuildingPartNumber+1).ToString();

            var room = roomNumber!="" && random.Next(2) == 0 ? "кв. " + roomNumber : "";

            var villageSoviet = villageSovets[random.Next(villageSovets.Count)];

            var area = areas[random.Next(areas.Count)];


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

            var stringAdress = new StringBuilder();
            foreach (var adresspart in adress)
            {
                if (adresspart.Length > 0)
                {
                    stringAdress.Append(adresspart + ", ");
                }
            }
            stringAdress.Remove(stringAdress.Length - 2, 2);

            return stringAdress;
        }

        protected override string generatePhoneCodeBeginningNumber()
        {
            var phoneCodesBeginningNumber = phoneCodesBeginningNumbers[random.Next(phoneCodesBeginningNumbers.Count)];
            return phoneCodesBeginningNumber;
        }
    }
}
