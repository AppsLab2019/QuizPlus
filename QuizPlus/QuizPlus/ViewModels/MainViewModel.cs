﻿using QuizPlus.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using QuizPlus.Views;
using Menu = QuizPlus.Views.Menu;

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
        public static List<Country> Countries { get; set; }

        public MainViewModel()
        {            
            CurrentRound = 1;
            _correctGuesses = 0;

            ChooseRandomCountries();
            CountryColors = new Color[] { Color.FromHex("#fff160"), Color.FromHex("#fff160"), Color.FromHex("#fff160"), Color.FromHex("#fff160") };

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

        readonly List<Country> InCorrectCountries = new List<Country>();

        private async Task HandleGameEnd()
        {
            string text = "Your incorrect guesses are ";
            int x = 0;

            int i = InCorrectCountries.Count();

            foreach (Country country in InCorrectCountries)
            {
                x += 1;

                if (country.Name == null)
                {
                    text += country.Capital;
                    if (x != i)
                        text += ", ";
                    else
                        text += ".";
                }
                else
                {
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
            }
            
            await Application.Current.MainPage.DisplayAlert("Congratulations", 
                $"You guessed {_correctGuesses} out of {MaxRounds} correctly! {text}", "Back");

            await Application.Current.MainPage.Navigation.PushAsync(new Menu());
        }

        private async Task ChangeColors(int answerIndex, bool correct)
        {

            if (correct)
                CountryColors[answerIndex] = Color.LightGreen;
            else
                CountryColors[answerIndex] = Color.FromHex("#ff6b6b");

            RaisePropertyChanged(nameof(CountryColors));
            await Task.Delay(850);
            CountryColors[answerIndex] = Color.FromHex("#fff160");
         
        }

        readonly List<Country> UsedCountries = new List<Country>();
        private void ChooseRandomCountries()
        {
            var rng = new Random();
            var countries = new List<Country>();

            while (countries.Count < 4)
            {               
                var country = MainViewModel.Countries[rng.Next(0, MainViewModel.Countries.Count)];

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
