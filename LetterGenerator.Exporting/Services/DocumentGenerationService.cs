using LetterGenerator.Exporting.Contracts;
using LetterGenerator.Exporting.Models;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
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
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public Task<string> GenerateWordAsync(ExportLetterDto letter)
        {
            return Task.Run(() =>
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
            });
        }

        public Task<string> GeneratePdfAsync(ExportLetterDto letter)
        {
            return Task.Run(() =>
            {
                var outputPath = Path.Combine(_templateDirectory, $"Letter_{letter.Number}.pdf");

                var fontPath = Path.Combine(_templateDirectory, "B-Nazanin.ttf");
                var imagePath = Path.Combine(_templateDirectory, "PdfTemplate.jpg");

                if (!File.Exists(fontPath))
                    throw new FileNotFoundException("Persian font file not found.", fontPath);

                if (!File.Exists(imagePath))
                    throw new FileNotFoundException("Header image not found.", imagePath);

                FontManager.RegisterFont(File.OpenRead(fontPath));

                var document = new LetterPdfDocument(letter, "B-Nazanin", imagePath);
                document.GeneratePdf(outputPath);

                return outputPath;
            });
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