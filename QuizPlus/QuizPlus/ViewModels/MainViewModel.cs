﻿using QuizPlus.Models;
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

        public Color[] CountryColors { get; private set; }

        public ICommand AnswerCommand { get; }

        private int _correctGuesses;
        private bool _isBusy;
        private readonly List<Country> _allCountries;

        public MainViewModel()
        {            
            CurrentRound = 1;
            _correctGuesses = 0;
            _allCountries = App.Countries;

            ChooseRandomCountries();
            CountryColors = new Color[] { Color.Gray, Color.Gray, Color.Gray, Color.Gray };

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
                await HandleGameEnd();

            RaiseAllPropertiesChanged();
            _isBusy = false;
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
            InCorrectCountries.Clear();

            ChooseRandomCountries();
        }

        private async Task ChangeColors(int answerIndex, bool correct)
        {
            if (correct)
                CountryColors[answerIndex] = Color.LightGreen;
            else
                CountryColors[answerIndex] = Color.Red;

            RaisePropertyChanged(nameof(CountryColors));
            await Task.Delay(1000);
            CountryColors[answerIndex] = Color.LightGray;
        }

        private void ChooseRandomCountries()
        {
            var rng = new Random();
            var countries = new List<Country>();

            while (countries.Count < 4)
            {               
                var country = _allCountries[rng.Next(0, _allCountries.Count)];

                if (!countries.Contains(country))
                    countries.Add(country);
            }

            CurrentCountries = countries.ToArray();
            CorrectCountry = countries[rng.Next(0, 4)];
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
