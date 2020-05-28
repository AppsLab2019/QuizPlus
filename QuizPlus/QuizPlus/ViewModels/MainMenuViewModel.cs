using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using QuizPlus.Models;
using QuizPlus.Views;
using Xamarin.Forms;
using System.Collections;

namespace QuizPlus.ViewModels
{
    public class MainMenuViewModel
    {
        public ICommand ButtonCommand { get; }

        public MainMenuViewModel()
        {
            ButtonCommand = new Command<string>(HandleButtonCommand);
        }

        public async void HandleButtonCommand(string button)
        {
            var buttonIndex = int.Parse(button);
            switch (buttonIndex)
            {
                case 0:
                    var allCountries = App.Countries;
                    MainViewModel._countries = allCountries;
                    break;
                case 1:
                    var europeCountries = App.Countries.Where(country => country.Category == Category.Europe).ToList();
                    MainViewModel._countries = europeCountries;
                    break;
                case 2:
                    var asiaCountries = App.Countries.Where(country => country.Category == Category.Asia).ToList();
                    MainViewModel._countries = asiaCountries;
                    break;
                case 3:
                    var americaCountries = App.Countries.Where(country => country.Category == Category.America).ToList();
                    MainViewModel._countries = americaCountries;
                    break;
                case 4:
                    var africaCountries = App.Countries.Where(country => country.Category == Category.Africa).ToList();
                    MainViewModel._countries = africaCountries;
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
