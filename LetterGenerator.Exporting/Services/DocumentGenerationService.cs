using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.Pdf;
using Syncfusion.DocIORenderer;
using LetterGenerator.Exporting.Contracts;
using LetterGenerator.Exporting.Models;

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
            string templatePath = Path.Combine(_templateDirectory, "LetterTemplateWithPlaceholders.docx");
            string outputPath = Path.Combine(_templateDirectory, $"Letter_{letter.Number}.docx");

            using FileStream inputStream = new(templatePath, FileMode.Open, FileAccess.Read);
            WordDocument document = new(inputStream, FormatType.Docx);

            document.Replace("{{Date}}", letter.DateTimeLocal.ToString("yyyy/MM/dd"), false, true);
            document.Replace("{{Number}}", letter.Number, false, true);
            document.Replace("{{RecipientTitleEntry}}", letter.RecipientName, false, true);
            document.Replace("{{RecipientPostionEntry}}", letter.RecipientPosition, false, true);
            document.Replace("{{Body}}", letter.Body, false, true);
            document.Replace("{{SenderNameEntry}}", letter.SenderName, false, true);
            document.Replace("{{SenderPostionEntry}}", letter.SenderPosition, false, true);
            document.Replace("{{Copy}}", letter.HaveCopy ? "رونوشت:\n" + letter.Copy : "", false, true);

            using FileStream outputStream = new(outputPath, FileMode.Create, FileAccess.Write);
            document.Save(outputStream, FormatType.Docx);
            document.Close();

            return outputPath;
        }

        public async Task<string> GeneratePdfAsync(ExportLetterDto letter)
        {
            var wordPath = await GenerateWordAsync(letter);
            var pdfPath = Path.Combine(_templateDirectory, $"Letter_{letter.Number}.pdf");

            using FileStream wordStream = new(wordPath, FileMode.Open, FileAccess.Read);
            using WordDocument document = new(wordStream, FormatType.Docx);

            using DocIORenderer renderer = new DocIORenderer();
            renderer.Settings.AutoTag = true;
            renderer.Settings.PreserveFormFields = true;

            using PdfDocument pdfDocument = renderer.ConvertToPDF(document);
            using FileStream pdfStream = new(pdfPath, FileMode.Create, FileAccess.Write);
            pdfDocument.Save(pdfStream);

            return pdfPath;
        }
    }
}