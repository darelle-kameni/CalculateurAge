using System.Collections.ObjectModel;
using CalculateurAge.Views;

namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private string _nom = string.Empty;
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = string.Empty;
    private bool _resultatVisible;
    private string _statut = string.Empty;
    private string _prochainAnniversaireText = string.Empty;
    private ObservableCollection<string> _historique = new();

    public string Nom
    {
        get => _nom;
        set
        {
            if (SetField(ref _nom, value))
                CalculerCommand.Rafraichir();
        }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set
        {
            if (SetField(ref _dateNaissance, value))
                CalculerCommand.Rafraichir();
        }
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    public string Statut
    {
        get => _statut;
        set => SetField(ref _statut, value);
    }

    public string ProchainAnniversaireText
    {
        get => _prochainAnniversaireText;
        set => SetField(ref _prochainAnniversaireText, value);
    }

    public ObservableCollection<string> Historique
    {
        get => _historique;
        set => SetField(ref _historique, value);
    }

    public DateTime DateNaissanceMax => DateTime.Today;

    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(Calculer, PeutCalculer);
        EffacerCommand = new RelayCommand(Effacer);
    }

    private bool PeutCalculer()
        => !string.IsNullOrWhiteSpace(Nom) && DateNaissance <= DateTime.Today;

    private void Effacer()
    {
        Nom = string.Empty;
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = string.Empty;
        Statut = string.Empty;
        ProchainAnniversaireText = string.Empty;
        ResultatVisible = false;
    }

    private void AjouterHistorique(string entree)
    {
        Historique.Insert(0, entree);
        if (Historique.Count > 10)
            Historique.RemoveAt(10);
    }

    private void Calculer()
    {
        if (!PeutCalculer())
        {
            Resultat = "Date de naissance invalide";
            Statut = string.Empty;
            ProchainAnniversaireText = string.Empty;
            ResultatVisible = true;
            return;
        }

        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-age))
            age--;

        string nom = Nom.Trim();
        string statut = age >= 18 ? "Majeur" : "Mineur";

        // Le 29 février n'existe pas les années non bissextiles.
        int annee = DateTime.Today.Year;
        int mois = DateNaissance.Month;
        int jour = DateNaissance.Day;

        if (mois == 2 && jour == 29 && !DateTime.IsLeapYear(annee))
            jour = 28;

        DateTime prochainAnniversaire = new DateTime(annee, mois, jour);
        if (prochainAnniversaire < DateTime.Today)
            prochainAnniversaire = prochainAnniversaire.AddYears(1);

        int joursRestants = (prochainAnniversaire.Date - DateTime.Today.Date).Days;

        Resultat = $"{nom}, vous avez {age} ans";
        Statut = statut;
        ProchainAnniversaireText = $"Prochain anniversaire dans {joursRestants} jours.";
        ResultatVisible = true;

        AjouterHistorique($"{nom} — {age} ans");

        _ = Shell.Current.GoToAsync(
            $"{nameof(ResultatPage)}?nom={Uri.EscapeDataString(nom)}&age={age}&statut={Uri.EscapeDataString(statut)}&prochain={Uri.EscapeDataString(ProchainAnniversaireText)}");
    }
}
