using System.Text;
using System;

namespace ReactTest.Utils
{
    public class USAUserDataGenerator : BaseUserDataGenerator
    {
        protected List<string> states;
        protected (int, int) phoneCodesBeginningNumbersRange;
        public USAUserDataGenerator(string dataFolder) : base(dataFolder)
        {
            states = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "states.csv"));
            alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            phoneCodesBeginningNumbersRange = (200, 950);
            phoneCodesBeginningFormats = new List<string> { "+1-x-","1-x-", "(1)x ","(x)"};

            phoneEndFormats = new List<string> { "xxx-xxxx", "xxxxxxx"};
            postIndexRange = (1, 99950);
        }

        protected override StringBuilder finishAdressGeneration(string index, string countryName, string livingPlace, TypeOfLivingPlace whatLivingPlaceType, string street, string buildingNumber, string roomNumber)
        {
            var indexUSA = index;
            for (int i = 0; i < 5 - index.Length; i++)
            {
                indexUSA = "0" + indexUSA;
            }
            var building = buildingNumber;

            var room = roomNumber;

            var stateOptionsToWrite = states[random.Next(states.Count)].Split(';');
            var state = stateOptionsToWrite[random.Next(stateOptionsToWrite.Length)];


            var adress = new StringBuilder();
            var adressFormat = random.Next(3);
            if (adressFormat < 2)//official post requirement
            {
                adress.Append(building + " ");
                adress.Append(street + ", ");
                if (room != "") 
                { 
                    adress.Append("Apartment #" + room + ", ");
                }
                adress.Append(livingPlace+", ");
                adress.Append(state + " " + indexUSA);
            }
            else
            {
                adress.Append(state+", ");
                adress.Append(livingPlace + ", ");
                adress.Append(building + " ");
                adress.Append(street + ", ");
                if (room != "")
                {
                    adress.Append("Ap. " + room + ", ");
                }
                adress.Append(indexUSA);
            }
            if (countryName != "")
            {
                adress.Append(", " + countryName);
            }

            return adress;
        }

        protected override string generatePhoneCodeBeginningNumber()
        {
            var phoneCodesBeginningNumber = random.Next(phoneCodesBeginningNumbersRange.Item1, phoneCodesBeginningNumbersRange.Item2 + 1).ToString();
            return phoneCodesBeginningNumber;
        }
    }
}
