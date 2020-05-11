using QuizPlus.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace QuizPlus.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        public int MaxRounds { get; } = 10;
        public int CurrentRound { get; private set; }

        public Country[] CurrentCountries { get; private set; }
        public Country CorrectCountry { get; private set; }

        public ICommand AnswerCommand { get; }

        private int _correctGuesses;

        public MainViewModel()
        {            
            CurrentRound = 1;
            _correctGuesses = 0;

            

            ChooseRandomCountries();

            AnswerCommand = new Command<string>(HandleAnswer);
        }
        
        private async void HandleAnswer(string button)
        {
            var buttonIndex = int.Parse(button);
            var playerChoice = CurrentCountries[buttonIndex];
        
            if (playerChoice == CorrectCountry)
                ++_correctGuesses;
            else
                InCorrectCountries.Add(CorrectCountry);

            if (MaxRounds >= ++CurrentRound)
                ChooseRandomCountries();
            else 
                await HandleGameEnd();

            RaiseAllPropertiesChanged();
        }
        List<Country> InCorrectCountries = new List<Country>();
       
        private async Task HandleGameEnd()
        {
            string text = "Your incorrect guesses are ";

            foreach(Country country in InCorrectCountries)
            {
                text += country.Name;
                text += " ";

            }

            await Application.Current.MainPage.DisplayAlert("Congratulations", 
                $"You guessed {_correctGuesses} out of {MaxRounds} correctly! {text}", "Reset");

            CurrentRound = 1;
            _correctGuesses = 0;

            ChooseRandomCountries();
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

            CurrentCountries = countries.ToArray();
            CorrectCountry = countries[rng.Next(0, 4)];
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
            new Country("Ukraine", "Kiev"),           
            new Country("Croatia", "Zagreb"),
            new Country("Mexico", "Mexico City"),
            new Country("Brazil", "Brasília"),
            new Country("Cuba", "Havana"),
            new Country("Japan", "Tokyo"),
            new Country("China", "Beijing"),
            new Country("Moldova", "Chișinău"),
            new Country("Zimbabwe", "Harare"),
            new Country("Russia", "Moscow"),
            new Country("Vietnam", "Hanoi"),
            new Country("Australia", "Canberra"),
            new Country("Egypt", "Cairo"),
            new Country("Kazakhstan", "	Nur-Sultan")
        };

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaiseAllPropertiesChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        #endregion
    }
}
