using LetterGenerator.Letter.Contracts;
using LetterGenerator.Letter.Models;

namespace LetterGenerator.MAUI;

public partial class LettersListPage : ContentPage
{
    private readonly ILetterService _letterService;

    public LettersListPage(ILetterService letterService)
    {
        InitializeComponent();
        _letterService = letterService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var letters = await _letterService.GetAllAsync();
        LettersList.ItemsSource = letters;
    }

    private async void OnLetterSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is LetterDto selectedLetter)
        {
            await Shell.Current.GoToAsync($"///EditLetter?number={selectedLetter.Number}");
        }

        LettersList.SelectedItem = null;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}