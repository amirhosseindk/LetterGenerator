using LetterGenerator.Exporting.Contracts;
using LetterGenerator.Exporting.Models;
using System.Text.RegularExpressions;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace LetterGenerator.Exporting.Services
{
    public class DocumentGenerationService : IDocumentGenerationService
    {
        private readonly string _templateDirectory;

        public DocumentGenerationService(string templateDirectory)
        {
            _templateDirectory = templateDirectory;
        }

        public async Task<string> GenerateWordAsync(ExportLetterDto letter)
        {
            var templatePath = Path.Combine(_templateDirectory, "LetterTemplateWithPlaceholders.docx");
            var outputPath = Path.Combine(_templateDirectory, $"Letter_{letter.Number}.docx");

            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Template file not found.", templatePath);

            using var doc = DocX.Load(templatePath);

            Replace(doc, "{{Date}}", letter.DateTimeLocal.ToString("yyyy/MM/dd"));
            Replace(doc, "{{Number}}", letter.Number);
            Replace(doc, "{{RecipientTitleEntry}}", letter.RecipientName);
            Replace(doc, "{{RecipientPostionEntry}}", letter.RecipientPosition);
            Replace(doc, "{{Body}}", letter.Body);
            Replace(doc, "{{SenderNameEntry}}", letter.SenderName);
            Replace(doc, "{{SenderPostionEntry}}", letter.SenderPosition);

            if (letter.HaveCopy)
                Replace(doc, "{{Copy}}", string.IsNullOrWhiteSpace(letter.Copy) ? "" : "رو نوشت:\n" + letter.Copy);

            foreach (var p in doc.Paragraphs)
                p.Font("B Nazanin");

            doc.SaveAs(outputPath);
            return outputPath;
        }

        public Task<string> GeneratePdfAsync(ExportLetterDto letter)
        {
            throw new NotSupportedException("PDF generation requires the commercial version of Xceed.");
        }

        private void Replace(DocX doc, string find, string replace)
        {
            var options = new StringReplaceTextOptions
            {
                NewValue = replace,
                SearchValue = find,
                RegExOptions = RegexOptions.None,
                TrackChanges = false
            };

            doc.ReplaceText(options);
        }
    }
}