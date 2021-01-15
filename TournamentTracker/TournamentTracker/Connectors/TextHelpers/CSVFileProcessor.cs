using System;
using System.IO;
using System.Linq;
using System.Configuration;
using TournamentTracker.Models;
using System.Collections.Generic;

namespace TournamentTracker.Connectors.TextHelpers
{
    public static class CSVFileProcessor
    {
        public static string GetFilePath(this string fileName) 
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                ConfigurationManager.AppSettings["DataFilesDirectory"];
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory + $"\\{fileName}";
        }

        public static List<string> LoadFile(this string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<string>();
            }

            return File.ReadAllLines(filePath).ToList();
        }

        public static void SaveToPrizesFile(this List<PrizeModel> prizes, string fileName)
        {
            var lines = new List<string>();

            foreach (var prize in prizes)
            {
                lines.Add($"{prize.Id},{prize.PlaceNumber},{prize.PlaceName},{prize.PrizeName},{prize.PrizeAmount},{prize.PrizePercentage}");
            }

            File.WriteAllLinesAsync(fileName.GetFilePath(), lines);
        }

        public static List<PrizeModel> ConvertLinesToPrizeModels(this List<string> lines)
        {
            var output = new List<PrizeModel>();

            foreach (var line in lines)
            {
                var cols = line.Split(',');

                output.Add(new PrizeModel()
                {
                    Id = int.Parse(cols[0]),
                    PlaceNumber = int.Parse(cols[1]),
                    PlaceName = cols[2],
                    PrizeName = cols[3],
                    PrizeAmount = decimal.Parse(cols[4]),
                    PrizePercentage = double.Parse(cols[5])
                });
            }

            return output;
        }

        public static void SaveToPeopleFile(this List<PersonModel> people, string fileName)
        {
            var lines = new List<string>();

            foreach (var person in people)
            {
                lines.Add($"{person.Id},{person.FirstName},{person.LastName},{person.EmailAddress},{person.CellphoneNumber}");
            }

            File.WriteAllLines(fileName.GetFilePath(), lines);
        }

        public static List<PersonModel> ConvertLinesToPersonModels(this List<string> lines)
        {
            var output = new List<PersonModel>();

            foreach (var line in lines)
            {
                var cols = line.Split(',');

                output.Add(new PersonModel
                {
                    Id = int.Parse(cols[0]),
                    FirstName = cols[1],
                    LastName = cols[2],
                    EmailAddress = cols[3],
                    CellphoneNumber = cols[4]
                });
            }

            return output;
        }
    }
}
