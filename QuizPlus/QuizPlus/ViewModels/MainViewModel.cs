using QuizPlus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace QuizPlus.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        public int MaxRounds { get; } = 15;
        public int CurrentRound { get; private set; }

        public Country[] CurrentCountries { get; private set; }
        public Country CorrectCountry { get; private set; }

        public ICommand AnswerCommand { get; }

        private int _correctGuesses;

        public MainViewModel()
        {
            // inicializácia na defaultné hodnoty
            CurrentRound = 1;
            _correctGuesses = 0;

            ChooseRandomCountries();

            AnswerCommand = new Command<string>(HandleAnswer);
        }

        // funkcia zavolaná po stlačení tlačidla
        private void HandleAnswer(string button)
        {
            // zmenenie textu reprezentujúceho dané tlačidlo na použiteľné číslo
            var buttonIndex = int.Parse(button);

            // kontrola výberu a zarátanie správnej odpovede
            if (CurrentCountries[buttonIndex] == CorrectCountry)
                ++_correctGuesses;

            // kontrola momentálneho kola
            if (MaxRounds > ++CurrentRound)
                ChooseRandomCountries();
            else 
                HandleGameEnd();

            RaiseAllPropertiesChanged();
        }

        // funkcia, ktorá zresetuje hru po poslednom kole
        private async void HandleGameEnd()
        {
            await Application.Current.MainPage.DisplayAlert("Congratulations", 
                $"You guessed {_correctGuesses} out of {MaxRounds} correctly!", "Reset");

            CurrentRound = 1;
            _correctGuesses = 0;

            ChooseRandomCountries();
        }

        // vygeneruje a nastaví náhodné štáty
        private void ChooseRandomCountries()
        {
            // inicializácia potrebných objektov
            var rng = new Random();
            var countries = new List<Country>();

            // TODO: lepšie implementovať
            // loop, pomocou ktorého zvolíme štyri náhodné štáty
            while (countries.Count < 4)
            {
                // náhodne zvolený štát zo všetkých dostupných
                var country = Countries[rng.Next(0, Countries.Count)];

                // kontrola duplikátov
                if (!countries.Contains(country))
                    countries.Add(country);
            }

            // nastavenie vygenerovaných štátov
            CurrentCountries = countries.ToArray();
            CorrectCountry = countries[rng.Next(0, 4)];
        }

        // list všetkých používaných štátov
        private List<Country> Countries = new List<Country>()
        {
            new Country("Armenia", "Jerevan"),
            new Country("Greece", "Athens"),
            new Country("Hungary", "Budapest"),
            new Country("Latvia", "Riga"),
            new Country("Netherlands", "Amsterdam"),
            new Country("Poland", "Warsaw"),
            new Country("Slovakia", "Bratislava"),
            new Country("Ukraine", "Kiev")
        };

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaiseAllPropertiesChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        #endregion
    }
}
