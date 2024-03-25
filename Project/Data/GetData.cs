using Project.Utility;

namespace Project.Data;

internal static class GetData
{
    /// <summary>
    /// This method reads edi raw files from the files folder
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static async Task<string> DataFromFile(string? fileName = "edi-835")
    {
        var data = await File.ReadAllTextAsync($"{FileHelpers.GetParentDirectory()}/Files/{fileName}.txt");

        return data;
    }

    /// <summary>
    /// This method reads the parsed data from the SavedJSONS folder
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static async Task<string> DataFromSegmentsJSON(string? fileName = "Segments")
    {
        var data = await File.ReadAllTextAsync($"{FileHelpers.GetParentDirectory()}/SavedJSONS/{fileName}.json");

        return data;
    }
}
