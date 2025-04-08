namespace LetterGenerator.MAUI;

public static class TemplateInitializer
{
    private static readonly string[] FilesToCopy = new[]
    {
        "LetterTemplateWithPlaceholders.docx",
        "pdftemplate.jpg",
        "B-Nazanin.ttf"
    };

    public static async Task EnsureTemplateCopiedAsync()
    {
        foreach (var fileName in FilesToCopy)
        {
            string destPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(destPath))
            {
                try
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                    using var output = File.Create(destPath);
                    await stream.CopyToAsync(output);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error copying {fileName}: {ex.Message}");
                }
            }
        }
    }
}
