namespace CalculateurAge.Views;

[QueryProperty(nameof(Nom), "nom")]
[QueryProperty(nameof(Age), "age")]
[QueryProperty(nameof(Statut), "statut")]
[QueryProperty(nameof(ProchainAnniversaire), "prochain")]
public partial class ResultatPage : ContentPage
{
    public string Nom { get; set; } = string.Empty;
    public string Age { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public string ProchainAnniversaire { get; set; } = string.Empty;

    public ResultatPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        lblMessage.Text = $"{Nom}, vous avez {Age} ans";
        lblStatut.Text = Statut;
        lblProchain.Text = ProchainAnniversaire;
    }

    private async void OnRetourClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
