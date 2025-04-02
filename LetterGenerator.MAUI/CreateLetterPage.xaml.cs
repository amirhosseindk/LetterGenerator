using LetterGenerator.Letter.Contracts;
using LetterGenerator.Letter.Models;

namespace LetterGenerator.MAUI;

public partial class CreateLetterPage : ContentPage
{
    private readonly ILetterService _letterService;

    public CreateLetterPage(ILetterService letterService)
    {
        InitializeComponent();
        _letterService = letterService;
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

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}