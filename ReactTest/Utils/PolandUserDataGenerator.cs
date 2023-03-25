using System;
using System.Text;

namespace ReactTest.Utils
{
    public class PolandUserDataGenerator : BaseUserDataGenerator
    {
        protected List<string> wojewodztwa;
        protected List<string> powiats;
        protected List<string> gminas;
        protected (int,int) phoneCodesBeginningNumbersRange;
        public PolandUserDataGenerator(string dataFolder) : base(dataFolder)
        {
            wojewodztwa = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "wojewodztwa.csv"));
            gminas = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "gminas.csv"));
            powiats = LoadSimpleOneRowFileData(Path.Combine(this.dataFolder, "powiats.csv"));
            alphabet = "AĄBCĆDEĘFGHIJKLŁMNŃOÓPQRSŚTUVWXYZŹŻaąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż";

            phoneCodesBeginningNumbersRange = (12,95);
            phoneCodesBeginningFormats = new List<string> { "+48 x ", "+48 x", "+48 (x) ", "+48-x-" };

            phoneEndFormats = new List<string> { "xxx xx xx", "xxx-xx-xx", "xxxxxxx" };
            postIndexRange = (1, 99440);
        }

        protected override StringBuilder finishAdressGeneration(string index, string countryName, string livingPlace, TypeOfLivingPlace whatLivingPlaceType, string street, string buildingNumber, string roomNumber)
        {
            var indexPoland = index;
            for (int i = 0;i< 5-index.Length;i++)
            {
                indexPoland = "0"+ indexPoland;
            }
            indexPoland = indexPoland.Substring(0, 2) + "-" + indexPoland.Substring(2);

            var building = buildingNumber;
            var part = random.Next(3) == 0 ? "" : ((char)('A'+random.Next(4))).ToString();

            var room = roomNumber != "" || random.Next(2) == 0 ? "m " + roomNumber : "";

            var wojewodztwo = wojewodztwa[random.Next(wojewodztwa.Count)];
            var powiat = powiats[random.Next(powiats.Count)];
            var gmina = gminas[random.Next(gminas.Count)];


            var adress = new StringBuilder();
            var adressFormat = random.Next(3);
            if (adressFormat == 0)//official post requirement
            {
                adress.Append(street.Replace("ul. ",""));
                adress.Append(" " + building+part);
                if (room != "")
                {
                    adress.Append(" " + room);
                }
                adress.Append(", " + indexPoland);
                adress.Append(" " + livingPlace);
            }
            else
            {
                if (adressFormat == 2)
                {
                    if (whatLivingPlaceType != TypeOfLivingPlace.majorCity)
                    {
                        adress.Append("w. " + wojewodztwo+", ");
                        adress.Append("p. " + powiat+", ");
                    }
                    if(whatLivingPlaceType == TypeOfLivingPlace.villageAndSimilar)
                        adress.Append("g. " + gmina+", ");
                    adress.Append(livingPlace);
                    adress.Append(", " + street);
                    adress.Append(", " + building + part);
                    if (room != "")
                    {
                        adress.Append(" " + room);
                    }
                    adress.Append(", " + indexPoland);
                }
                else
                {
                    if (whatLivingPlaceType != TypeOfLivingPlace.majorCity)
                    {
                        adress.Append("w. " + wojewodztwo+ ", ");
                    }
                    adress.Append(street);
                    adress.Append(", " + building + part);
                    if (room != "")
                    {
                        adress.Append(", " + room);
                    }
                    if (whatLivingPlaceType == TypeOfLivingPlace.villageAndSimilar)
                    {
                        adress.Append(", W. ");
                    }
                    else
                    {
                        adress.Append(", ");
                    }
                    adress.Append(livingPlace);
                    
                    adress.Append(", " + indexPoland);
                }
            }
            if (countryName != "")
            {
                adress.Append(" " + countryName);
            }

            return adress;
        }

        protected override string generatePhoneCodeBeginningNumber()
        {
            var phoneCodesBeginningNumber = random.Next(phoneCodesBeginningNumbersRange.Item1, phoneCodesBeginningNumbersRange.Item2+1).ToString();
            return phoneCodesBeginningNumber;
        }
    }
}
