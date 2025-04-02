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
        await LoadLettersAsync();
    }

    private async Task LoadLettersAsync()
    {
        var letters = await _letterService.GetAllAsync();
        ButtonsContainer.Children.Clear();

        foreach (var letter in letters)
        {
            var button = new Button
            {
                Text = $"نامه شماره {letter.Number}",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            button.Clicked += (sender, args) => OnLetterButtonClicked(letter.Number);
            ButtonsContainer.Children.Add(button);
        }
    }

    private async void OnLetterButtonClicked(string letterNumber)
    {
        await Shell.Current.GoToAsync($"///EditLetter?number={letterNumber}");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}