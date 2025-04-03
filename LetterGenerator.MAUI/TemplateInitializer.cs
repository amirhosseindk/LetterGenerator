namespace LetterGenerator.MAUI
{
    public static class TemplateInitializer
    {
        public static async Task EnsureTemplateCopiedAsync()
        {
            string fileName = "LetterTemplateWithPlaceholders.docx";
            string destPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(destPath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                using var output = File.Create(destPath);
                await stream.CopyToAsync(output);
            }
        }
    }
}