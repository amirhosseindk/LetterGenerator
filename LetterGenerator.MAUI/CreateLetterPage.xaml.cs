using LetterGenerator.Letter.Contracts;
using LetterGenerator.Letter.Models;
using LetterGenerator.Exporting.Contracts;
using LetterGenerator.Exporting.Models;

namespace LetterGenerator.MAUI;

public partial class CreateLetterPage : ContentPage
{
    private readonly ILetterService _letterService;
    private readonly IDocumentGenerationService _docService;

    public CreateLetterPage(ILetterService letterService, IDocumentGenerationService docService)
    {
        InitializeComponent();
        _letterService = letterService;
        _docService = docService;
    }

    private async void OnSaveLetterClicked(object sender, EventArgs e)
    {
        var dto = new CreateLetterDto
        {
            Number = DateNumberEntry.Text ?? "",
            DateTimeLocal = DateTime.Now,
            RecipientName = RecipientTitleEntry.Text ?? "",
            RecipientPosition = RecipientPostionEntry.Text ?? "",
            Body = BodyEditor.Text ?? "",
            SenderName = SenderNameEntry.Text ?? "",
            SenderPosition = SenderPostionEntry.Text ?? "",
            HaveCopy = !string.IsNullOrWhiteSpace(CopyEntry.Text),
            Copy = CopyEntry.Text ?? "",
            Username = "TestUser",
            DeviceType = Shared.Types.DeviceType.Android
        };

        var result = await _letterService.CreateAsync(dto);
        await DisplayAlert("نتیجه", result ? "نامه با موفقیت ذخیره شد!" : "خطا در ذخیره‌سازی نامه.", "باشه");
    }

    private async void OnExportWordClicked(object sender, EventArgs e)
    {
        var dto = GetExportDtoFromInput();
        var path = await _docService.GenerateWordAsync(dto);
        await Launcher.Default.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(path) });
    }

    private async void OnExportPdfClicked(object sender, EventArgs e)
    {
        var dto = GetExportDtoFromInput();
        var path = await _docService.GeneratePdfAsync(dto);
        await Launcher.Default.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(path) });
    }

    private ExportLetterDto GetExportDtoFromInput()
    {
        return new ExportLetterDto
        {
            Number = DateNumberEntry.Text ?? "",
            DateTimeLocal = DateTime.Now,
            RecipientName = RecipientTitleEntry.Text ?? "",
            RecipientPosition = RecipientPostionEntry.Text ?? "",
            Body = BodyEditor.Text ?? "",
            SenderName = SenderNameEntry.Text ?? "",
            SenderPosition = SenderPostionEntry.Text ?? "",
            HaveCopy = !string.IsNullOrWhiteSpace(CopyEntry.Text),
            Copy = CopyEntry.Text ?? ""
        };
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}