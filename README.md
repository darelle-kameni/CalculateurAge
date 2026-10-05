# CalculateurAge

Application mobile .NET MAUI qui calcule l'âge d'une personne à partir de
son nom et de sa date de naissance.

Realise dans le cadre d'un TP sur le **routing** et l'architecture
**MVVM** avec .NET MAUI.

## Fonctionnalites

- Saisie du **nom** et de la **date de naissance**
- Calcul de l'age reel (ajuste les annees bissextiles)
- Affichage du **statut** majeur / mineur
- Affichage du **prochain anniversaire** en nombre de jours
- **Historique** des 10 derniers calculs
- Bouton **Effacer** pour reinitialiser le formulaire
- Refus des dates de naissance dans le futur
- Bouton **Calculer** desactive tant que le nom est vide
- Navigation vers une seconde page de resultat via **Shell Routing**

## Architecture

Le projet suit le patron **MVVM** : la vue ne contient aucune logique
metier, toute la logique vit dans le ViewModel.

```text
     View (XAML)
         |
    Binding / Command
         v
      ViewModel
         |
    Logique metier
         v
        Model
```

### Arborescence

```text
CalculateurAge/
|
|-- Views/
|   |-- ResultatPage.xaml
|   `-- ResultatPage.xaml.cs
|
|-- ViewModels/
|   |-- BaseViewModel.cs
|   |-- RelayCommand.cs
|   `-- CalculateurViewModel.cs
|
|-- MainPage.xaml
|-- MainPage.xaml.cs
|-- AppShell.xaml
|-- AppShell.xaml.cs
|-- App.xaml
|-- App.xaml.cs
`-- MauiProgram.cs
```

| Fichier | Role |
| --- | --- |
| `ViewModels/BaseViewModel.cs` | Implemente `INotifyPropertyChanged` et la methode `SetField` pour notifier les changements. |
| `ViewModels/RelayCommand.cs` | Encapsule une action derriere `ICommand` pour pouvoir la lier a un `Button`. |
| `ViewModels/CalculateurViewModel.cs` | Toute la logique metier : calcul de l'age, statut, historique, navigation. |
| `Views/ResultatPage.xaml.cs` | Page de resultat, alimentee par les `[QueryProperty]` du routing Shell. |
| `AppShell.xaml.cs` | Enregistre la route `ResultatPage` via `Routing.RegisterRoute`. |

## Navigation

La route est declaree dans `AppShell.xaml.cs` :

```csharp
Routing.RegisterRoute(nameof(ResultatPage), typeof(ResultatPage));
```

Le ViewModel navigue vers la page de resultat en passant les donnees
encodees dans l'URL :

```csharp
await Shell.Current.GoToAsync(
    $"{nameof(ResultatPage)}?nom={Uri.EscapeDataString(nom)}&age={age}");
```

Retour vers la page precedente :

```csharp
await Shell.Current.GoToAsync("..");
```

Les valeurs recus par la page sont exposees grace aux attributs
`[QueryProperty]`, puis affichees dans `OnAppearing()` car elles sont
remplies apres le constructeur.

## Lancer le projet

```bash
dotnet workload install maui-android
dotnet build
```

Cible : `net10.0-android`. Lancez l'emulateur Android depuis
Visual Studio, ou :

```bash
dotnet build -t:Run -f net10.0-android
```

## Notes techniques

- Les ViewModels ne contiennent aucun controle d'interface
  (`Label`, `Entry`, `Button`, `DisplayAlert`).
- `SetField` evite de notifier une propriete dont la valeur n'a pas
  change.
- `RelayCommand.Rafraichir()` declenche `CanExecuteChanged` pour
  activer ou desactiver le bouton selon l'etat du formulaire.
