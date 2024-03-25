namespace Project.Utility;

internal static class FileHelpers
{

    /// <summary>
    /// Get the parent directory of the project
    /// </summary>
    /// <returns></returns>
    public static string GetParentDirectory()
    {
        string rootDirectory = Directory.GetCurrentDirectory();
        Enumerable.Range(0, 3).ToList().ForEach((e) =>
        {
            rootDirectory = Directory.GetParent(rootDirectory).FullName;
        });
        return rootDirectory;
    }

    /// <summary>
    /// Save file on the path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static async Task SaveTextFile(string path, string text)
    {
        await File.WriteAllTextAsync($"{GetParentDirectory()}/{path}", text);
    }

    public static async Task<string> ReadTextFile(string? path)
    {
        var data = await File.ReadAllTextAsync($"{GetParentDirectory()}/{path}");

        return data;
    }
}
