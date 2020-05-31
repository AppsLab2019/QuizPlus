using QuizPlus.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace QuizPlus.ViewModels
{

    public class MenuViewModel
    {
        public ICommand ButtonCommand { get; }

        public MenuViewModel()
        {
            ButtonCommand = new Command<string>(HandleButtonCommand);
        }


        public void HandleButtonCommand(string button)
        {
            
            if (button == "Capitals")
            {
                Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
            }
            else if (button == "Football")
            {
                Application.Current.MainPage.Navigation.PushAsync(new FootballMenu());
            }

        }

    }
}
