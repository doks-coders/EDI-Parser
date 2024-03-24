namespace Project.Utility;

internal static class FileHelpers
{
    public static string GetParentDirectory()
    {
        string rootDirectory = Directory.GetCurrentDirectory();
        Enumerable.Range(0, 3).ToList().ForEach((e) =>
        {
            rootDirectory = Directory.GetParent(rootDirectory).FullName;
        });
        return rootDirectory;
    }

    public static async Task SaveTextFile(string path, string text)
    {
        await File.WriteAllTextAsync($"{GetParentDirectory()}/{path}", text);
    }
}
