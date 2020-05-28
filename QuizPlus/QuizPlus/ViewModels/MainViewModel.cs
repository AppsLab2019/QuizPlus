using QuizPlus.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace QuizPlus.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        public int MaxRounds { get; } = 10;
        public int CurrentRound { get; private set; }

        public Country[] CurrentCountries { get; private set; }
        public Country CorrectCountry { get; private set; }

        public Color[] CountryColors { get; private set; }

        public ICommand AnswerCommand { get; }

        private int _correctGuesses;
        private bool _isBusy;
        public static List<Country> _countries { get; set; }

        public MainViewModel()
        {            
            CurrentRound = 1;
            _correctGuesses = 0;

            ChooseRandomCountries();
            CountryColors = new Color[] { Color.Default, Color.Default, Color.Default, Color.Default };

            AnswerCommand = new Command<string>(HandleAnswer);
        }
        
        private async void HandleAnswer(string button)
        {
            if (_isBusy)
                return;

            _isBusy = true;

            var buttonIndex = int.Parse(button);
            var playerChoice = CurrentCountries[buttonIndex];
            var isRight = playerChoice == CorrectCountry;

            if (isRight)
                ++_correctGuesses;
            else
                InCorrectCountries.Add(CorrectCountry);

            await ChangeColors(buttonIndex, isRight);

            if (MaxRounds >= ++CurrentRound)
                ChooseRandomCountries();
            else
            {
                await HandleGameEnd();
                return;
            }

            RaiseAllPropertiesChanged();
            _isBusy = false;
        }
        List<Country> InCorrectCountries = new List<Country>();

        private async Task HandleGameEnd()
        {
            string text = "Your incorrect guesses are ";
            int x = 0;

             int i = InCorrectCountries.Count();

            foreach(Country country in InCorrectCountries)
            {
                x += 1;
                if (x != i)
                {
                    text += country.Name;
                    text += ", ";
                }
                else 
                {
                    text += country.Name;
                    text += ".";
                }
            }
            
            await Application.Current.MainPage.DisplayAlert("Congratulations", 
                $"You guessed {_correctGuesses} out of {MaxRounds} correctly! {text}", "Back");

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task ChangeColors(int answerIndex, bool correct)
        {   
            
            if (correct)
                CountryColors[answerIndex] = Color.FromHex("#1f5414");
            else
                CountryColors[answerIndex] = Color.Red;

            RaisePropertyChanged(nameof(CountryColors));
            await Task.Delay(850);
            CountryColors[answerIndex] = Color.LightGray;
         
        }

        List<Country> UsedCountries = new List<Country>();
        private void ChooseRandomCountries()
        {
            var rng = new Random();
            var countries = new List<Country>();

            while (countries.Count < 4)
            {               
                var country = _countries[rng.Next(0, _countries.Count)];

                if (UsedCountries.Contains(country))
                    continue; 

                if (!countries.Contains(country))
                    countries.Add(country);
            }
            
            CurrentCountries = countries.ToArray();
            var randomCorrectCountry = countries[rng.Next(0, 4)];
            UsedCountries.Add(randomCorrectCountry);
            CorrectCountry = randomCorrectCountry;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void RaiseAllPropertiesChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        #endregion
    }
}
