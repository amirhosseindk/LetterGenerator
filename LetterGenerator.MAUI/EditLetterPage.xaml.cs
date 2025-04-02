using LetterGenerator.Letter.Contracts;
using LetterGenerator.Letter.Models;

namespace LetterGenerator.MAUI;

[QueryProperty(nameof(LetterNumber), "number")]
public partial class EditLetterPage : ContentPage
{
    private readonly ILetterService _letterService;
    private LetterDto _letter;

    public string LetterNumber { get; set; }

    public EditLetterPage(ILetterService letterService)
    {
        InitializeComponent();
        _letterService = letterService;
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

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///LettersList");
    }
}