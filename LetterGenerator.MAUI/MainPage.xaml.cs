namespace LetterGenerator.MAUI;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCreateLetterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///CreateLetter");
    }

    private async void OnViewLettersClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///LettersList");
    }
}