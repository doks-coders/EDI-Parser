using Project.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data
{
    internal static class GetData
    {
        public static async Task<string> DataFromFile(string? fileName="edi-835")
        {
            var data = await File.ReadAllTextAsync($"{FileHelpers.GetParentDirectory()}/Files/{fileName}.txt");

            return data;
        }

        /// <summary>
        /// This method reads the parsed data from the SavedJSONS folder
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<string> DataFromSegmentsJSON(string? fileName = "SegmentsNew")
        {
            var data = await File.ReadAllTextAsync($"{FileHelpers.GetParentDirectory()}/SavedJSONS/{fileName}.json");

            return data;
        }
    }
}
