using QuizPlus.Models;
using QuizPlus.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace QuizPlus.ViewModels
{
    public class FootballMenuViewModel
    {

        public ICommand ButtonCommand {get; }

        public FootballMenuViewModel()
        {
            ButtonCommand = new Command<string>(HandleButtonCommand);
        }

        public async void HandleButtonCommand(string button)
        {
            var buttonIndex = int.Parse(button);
            switch (buttonIndex)
            {
                case 0:
                    var premierLeagueClubs = App.Countries.Where(club => club.Category == Category.PremierLeague).ToList();
                    MainViewModel._countries = premierLeagueClubs;
                    break;
                default:
                    break;
            }

            await PushAsync(new MainPage());
        }
        private Task PushAsync(Page page)
        {
            var navigation = Application.Current.MainPage.Navigation;
            return navigation.PushAsync(page);
        }


    }
}
