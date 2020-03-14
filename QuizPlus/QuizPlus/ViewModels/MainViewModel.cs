using QuizPlus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace QuizPlus.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int MaxRounds { get; } = 15;
        public int CurrentRound { get; private set; }

        public ImageSource CountryImage { get; private set; }
        public string CountryName { get; private set; }

        public string FirstCountry => _currentCountries[0].Capital;
        public string SecondCountry => _currentCountries[1].Capital;
        public string ThirdCountry => _currentCountries[2].Capital;
        public string FourthCountry => _currentCountries[3].Capital;

        public ICommand AnswerCommand { get; }

        private Country[] _currentCountries;
        private Country _correctCountry;
        private int _correctGuesses;

        public MainViewModel()
        {
            CurrentRound = 1;
            _correctGuesses = 0;

            ChooseRandomCountries();

            AnswerCommand = new Command<string>(HandleAnswer);
        }

        private void HandleAnswer(string button)
        {
            var buttonIndex = int.Parse(button);

            if (_currentCountries[buttonIndex] == _correctCountry)
                ++_correctGuesses;

            if (++CurrentRound <= MaxRounds)
            {
                ChooseRandomCountries();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(default));
            }
            else HandleGameEnd();
        }

        private async void HandleGameEnd()
        {
            await Application.Current.MainPage.DisplayAlert("Congratulations", 
                $"You guessed {_correctGuesses} out of {MaxRounds} correctly!", "Ok");

            CurrentRound = 1;
            ChooseRandomCountries();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(default));
        }

        private void ChooseRandomCountries()
        {
            var rng = new Random();
            var countries = new List<Country>();

            while (countries.Count < 4)
            {
                var country = Countries[rng.Next(0, Countries.Count)];

                if (!countries.Contains(country))
                    countries.Add(country);
            }

            _currentCountries = countries.ToArray();
            _correctCountry = countries[rng.Next(0, 4)];

            CountryImage = ImageSource.FromFile($"{_correctCountry.Name}.png");
            CountryName = _correctCountry.Name;
        }

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
    }
}
