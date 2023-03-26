using GenerateFakeUserData.Models;
using System.Text;

namespace GenerateFakeUserData.Utils
{
    public abstract class BaseUserDataGenerator
    {
        protected List<string> maleFirstNames;
        protected List<string> maleSurNames;
        protected List<string> malePatronymicsNames;//empty if is not used

        protected List<string> femaleFirstNames;
        protected List<string> femaleSurNames;//empty if there is no difference with males
        protected List<string> femalePatronymicsNames;//empty if is not used

        protected string dataFolder;

        protected List<string> majorCities;

        protected List<string> allCities;
        protected List<string> villagesAndSimilar;
        protected List<string> streets;

        protected string alphabet;

        protected List<string> phoneCodesBeginningFormats;
        protected List<string> phoneEndFormats;

        protected (int, int) postIndexRange;

        protected List<string> possibleCountryNames;

        protected Random random;

        public BaseUserDataGenerator(string dataFolder)
        {
            this.dataFolder = dataFolder;
            maleFirstNames = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "male names.txt"));
            maleSurNames = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "male surnames.txt"));
            malePatronymicsNames = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "male patronymics.txt"), skipIfNotExists: true);

            femaleFirstNames = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "female names.txt"));
            femaleSurNames = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "female surnames.txt"), skipIfNotExists: true);
            femalePatronymicsNames = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "female patronymics.txt"), skipIfNotExists: true);

            allCities = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "cities.csv"));
            villagesAndSimilar = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "villages.csv"));
            streets = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "streets.csv"));
            majorCities = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "majorCities.csv"));

            possibleCountryNames = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "countryNames.csv"));
            possibleCountryNames.Add("");//there could be no country name at all
        }

        private StringBuilder GenerateAdress()
        {
            int maxBuildingNumber = 100;
            int maxRoomNumber = 80;

            var countryName = possibleCountryNames[random.Next(possibleCountryNames.Count)];
            var street = streets[random.Next(streets.Count)];

            var whatLivingPlaceType = (TypeOfLivingPlace)random.Next((int)TypeOfLivingPlace.villageAndSimilar + 1); ;
            var livingPlace = "";
            switch (whatLivingPlaceType)
            {
                case TypeOfLivingPlace.majorCity:
                    livingPlace = majorCities[random.Next(majorCities.Count)];
                    break;
                case TypeOfLivingPlace.usualCity:
                    livingPlace = allCities[random.Next(allCities.Count)];
                    break;
                default:
                    livingPlace = villagesAndSimilar[random.Next(villagesAndSimilar.Count)];
                    break;
            }
            var index = random.Next(postIndexRange.Item1, postIndexRange.Item2).ToString();
            var buildingNumber = random.Next(1, maxBuildingNumber + 1).ToString();
            var roomNumber = random.Next(2) == 0 ? random.Next(1, maxRoomNumber + 1).ToString() : "";

            var stringAdress = finishAdressGeneration(index, countryName, livingPlace, whatLivingPlaceType, street, buildingNumber, roomNumber);
            return stringAdress;
        }

        private StringBuilder GeneratePhoneNumber()
        {
            var phoneCodesBeginningFormat = phoneCodesBeginningFormats[random.Next(phoneCodesBeginningFormats.Count)];
            var phoneCodesBeginningNumber = generatePhoneCodeBeginningNumber();
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
            var phoneNumber = new StringBuilder();
            phoneNumber.Append(phoneCodesBeginning);
            phoneNumber.Append(phoneEnd);
            return phoneNumber;
        }

        private StringBuilder GenerateFullName()
        {
            var fullName = new StringBuilder();

            bool isMale = random.Next(2) == 0;
            if (isMale)
            {
                fullName.Append(getRandomElement(maleSurNames, random) + " " + getRandomElement(maleFirstNames, random));
                if (malePatronymicsNames.Count > 0)
                    fullName.Append(" " + getRandomElement(malePatronymicsNames, random));
            }
            else
            {
                if (femaleSurNames.Count > 0)
                {
                    fullName.Append(getRandomElement(femaleSurNames, random));
                }
                else
                {
                    fullName.Append(getRandomElement(maleSurNames, random));
                }
                fullName.Append(" " + getRandomElement(femaleFirstNames, random));
                if (femalePatronymicsNames.Count > 0)
                {
                    fullName.Append(" " + getRandomElement(femalePatronymicsNames, random));
                }
                else
                {
                    if (malePatronymicsNames.Count > 0)
                    {
                        fullName.Append(" " + getRandomElement(malePatronymicsNames, random));
                    }
                }
            }
            return fullName;
        }

        private void IntroduceErrors(StringBuilder fullName, StringBuilder stringAdress, StringBuilder phoneNumber, double errorsPerRecord)
        {
            int intPartOfErrorsPerRecord = (int)errorsPerRecord;
            double floatPartOfErrorsPerRecord = errorsPerRecord - intPartOfErrorsPerRecord;
            int intErrorsPerRecord = intPartOfErrorsPerRecord;
            var rnd01 = random.NextDouble();
            if (rnd01 < floatPartOfErrorsPerRecord)
            {
                intErrorsPerRecord += 1;
            }

            var changedData = new StringBuilder[] { fullName, stringAdress, phoneNumber };
            for (int i = 0; i < intErrorsPerRecord; i++)
            {
                int errorLocation = random.Next(3);

                int errorType = random.Next(3);
                int errorPosition = random.Next(changedData[errorLocation].Length);
                switch (errorType)
                {
                    case 0://remove
                        if (changedData[errorLocation].Length > 0)
                        {
                            changedData[errorLocation].Remove(errorPosition, 1);
                        }
                        break;
                    case 1://add
                        var symbolToAdd = errorLocation == 2 ? random.Next(10).ToString() : alphabet[random.Next(alphabet.Length)].ToString();
                        changedData[errorLocation].Insert(errorPosition, symbolToAdd);
                        break;
                    default://swap
                        if (changedData[errorLocation].Length > 1)
                        {
                            if (errorPosition == changedData[errorLocation].Length - 1)
                            {
                                errorPosition = errorPosition - 1;
                            }
                            var symbolFirst = changedData[errorLocation][errorPosition];
                            var symbolSecond = changedData[errorLocation][errorPosition + 1];
                            changedData[errorLocation].Remove(errorPosition, 2);
                            changedData[errorLocation].Insert(errorPosition, symbolSecond);
                            changedData[errorLocation].Insert(errorPosition, symbolFirst);
                        }
                        break;
                }
            }
        }

        public UserDataModel GenerateRecord(double errorsPerRecord, int seed, bool isRegenerateSeed, out int newSeed)
        {
            var recordSeed = seed;
            random = new Random(recordSeed);
            newSeed = random.Next();
            if(isRegenerateSeed)
            {
                recordSeed = newSeed;
                random = new Random(recordSeed);
                newSeed = random.Next();
            }

            var stringAdress = GenerateAdress();

            var phoneNumber = GeneratePhoneNumber();

            var fullName = GenerateFullName();

            IntroduceErrors(fullName, stringAdress, phoneNumber, errorsPerRecord);

            var resultRecord = new UserDataModel()
            {
                number = -1,//will be set in calling method
                randomId = recordSeed,
                fullName = fullName.ToString(),
                adress = stringAdress.ToString(),
                phone = phoneNumber.ToString()
            };
            return resultRecord;
        }
        protected abstract StringBuilder finishAdressGeneration(string index, string countryName, string livingPlace, TypeOfLivingPlace whatLivingPlaceType, string street, string buildingNumber, string roomNumber);

        protected abstract string generatePhoneCodeBeginningNumber();

        protected List<string> LoadSimpleOneRowFileData(string filePath, bool skipIfNotExists=false)
        {
            var result = new List<string>();
            if (File.Exists(filePath) || !skipIfNotExists)
            {
                var csvRows = System.IO.File.ReadAllLines(filePath, Encoding.UTF8).ToList();
                foreach (var row in csvRows)
                {
                    var rowTrim = row.Trim();
                    if (rowTrim != "")
                        result.Add(row);
                }
            }
            return result;
        }

        protected string getRandomElement(List<string> array, Random random)
        {
            var result = array[random.Next(array.Count)];
            return result;
        }
    }

    public enum TypeOfLivingPlace { majorCity, usualCity, villageAndSimilar };
}
