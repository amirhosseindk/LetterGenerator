using LetterGenerator.Exporting.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

namespace LetterGenerator.Exporting.Services
{
    public class LetterPdfDocument : IDocument
    {
        private readonly ExportLetterDto _letter;
        private readonly string _fontName;
        private readonly string _headerImagePath;

        public LetterPdfDocument(ExportLetterDto letter, string fontName, string headerImagePath)
        {
            _letter = letter;
            _fontName = fontName;
            _headerImagePath = headerImagePath;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(0);

                page.DefaultTextStyle(TextStyle.Default
                    .FontFamily(_fontName)
                    .FontSize(13)
                    .DirectionFromRightToLeft());

                page.Background()
                    .Image(_headerImagePath)
                    .FitArea();

                page.Content()
                    .PaddingHorizontal(60)
                    .PaddingVertical(100)
                    .Column(col =>
                    {
                        col.Spacing(12);
                        col.Item().Text($"تاریخ: {_letter.DateTimeLocal:yyyy/MM/dd}");
                        col.Item().Text($"شماره: {_letter.Number}");
                        col.Item().Text($"گیرنده: {_letter.RecipientName} ({_letter.RecipientPosition})");
                        col.Item().Text("متن نامه:");
                        col.Item().Text(_letter.Body);
                        col.Item().Text($"ارسال‌کننده: {_letter.SenderName} ({_letter.SenderPosition})");

                        if (_letter.HaveCopy && !string.IsNullOrWhiteSpace(_letter.Copy))
                            col.Item().Text($"رو نوشت:\n{_letter.Copy}");
                    });
            });
        }
    }
}