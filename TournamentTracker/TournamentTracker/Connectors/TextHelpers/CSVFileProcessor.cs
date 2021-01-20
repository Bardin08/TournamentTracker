using System;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

namespace TournamentTracker.Connectors.TextHelpers
{
    public static class CSVFileProcessor
    {
        /// <summary>
        /// Allows to get a file path by file name.
        /// </summary>
        /// <param name="fileName"> File name </param>
        /// <remarks>
        /// Returns "%appdata%/TournamentTracker/Data/FILE NAME".
        /// </remarks>
        /// <returns> File path</returns>
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

        /// <summary>
        /// Return all the data from file.
        /// </summary>
        /// <param name="filePath"> File path, can be received with <seealso cref="GetFilePath(string)"/></param>
        public static List<string> LoadFile(this string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<string>();
            }

            return File.ReadAllLines(filePath).ToList();
        }

        /// <summary>
        /// Writes data to the file.
        /// </summary>
        /// <param name="lines"> Data that will be written </param>
        /// <param name="fileName"> File path, can be received with <seealso cref="GetFilePath(string)"/> </param>
        public static void SaveToFile(this List<string> lines, string filePath)
        {
            if (lines == null)
            {
                return;
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
