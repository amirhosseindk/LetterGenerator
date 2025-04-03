using LetterGenerator.Letter.Contracts;
using LetterGenerator.Letter.Models;
using LetterGenerator.Exporting.Contracts;
using LetterGenerator.Exporting.Models;

namespace LetterGenerator.MAUI;

[QueryProperty(nameof(LetterNumber), "number")]
public partial class EditLetterPage : ContentPage
{
    private readonly ILetterService _letterService;
    private readonly IDocumentGenerationService _docService;
    private LetterDto _letter;

    public string LetterNumber { get; set; }

    public EditLetterPage(ILetterService letterService, IDocumentGenerationService docService)
    {
        InitializeComponent();
        _letterService = letterService;
        _docService = docService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (string.IsNullOrEmpty(LetterNumber))
        {
            await DisplayAlert("خطا", "شماره نامه ارسال نشده است!", "باشه");
            await Shell.Current.GoToAsync("///LettersList");
            return;
        }

        _letter = await _letterService.GetByNumberAsync(LetterNumber);

        DateNumberEntry.Text = _letter.Number;
        BodyEditor.Text = _letter.Body;
        RecipientTitleEntry.Text = _letter.RecipientName;
        RecipientPostionEntry.Text = _letter.RecipientPosition;
        SenderNameEntry.Text = _letter.SenderName;
        SenderPostionEntry.Text = _letter.SenderPosition;
        CopyEntry.Text = _letter.Copy;
    }

    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        var dto = new UpdateLetterDto
        {
            Id = _letter.Id,
            Number = DateNumberEntry.Text,
            Body = BodyEditor.Text,
            RecipientName = RecipientTitleEntry.Text,
            RecipientPosition = RecipientPostionEntry.Text,
            SenderName = SenderNameEntry.Text,
            SenderPosition = SenderPostionEntry.Text,
            Copy = CopyEntry.Text,
            HaveCopy = !string.IsNullOrWhiteSpace(CopyEntry.Text),
            DateTimeLocal = _letter.DateTimeLocal,
            ModifiedDateTimeUtc = DateTime.UtcNow,
            Username = "TestUser",
            DeviceType = Shared.Types.DeviceType.Android
        };

        var result = await _letterService.UpdateAsync(dto);
        await DisplayAlert("نتیجه", result ? "تغییرات ذخیره شد!" : "خطا در ذخیره‌سازی", "باشه");
    }

    private async void OnExportWordClicked(object sender, EventArgs e)
    {
        try
        {
            var dto = MapToExportDto(_letter);
            var path = await _docService.GenerateWordAsync(dto);
            await Launcher.Default.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(path) });
        }
        catch (Exception ex)
        {
            await DisplayAlert("خطا", ex.Message, "باشه");
        }
    }

    private async void OnExportPdfClicked(object sender, EventArgs e)
    {
        try
        {
            var dto = MapToExportDto(_letter);
            var path = await _docService.GeneratePdfAsync(dto);
            await Launcher.Default.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(path) });
        }
        catch (Exception ex)
        {
            await DisplayAlert("خطا", ex.Message, "باشه");
        }
    }

    private static ExportLetterDto MapToExportDto(LetterDto letter)
    {
        return new ExportLetterDto
        {
            Number = letter.Number,
            DateTimeLocal = letter.DateTimeLocal,
            RecipientName = letter.RecipientName,
            RecipientPosition = letter.RecipientPosition,
            Body = letter.Body,
            SenderName = letter.SenderName,
            SenderPosition = letter.SenderPosition,
            HaveCopy = letter.HaveCopy,
            Copy = letter.Copy
        };
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///LettersList");
    }
}