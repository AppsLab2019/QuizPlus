using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using QuizPlus.Models;
using QuizPlus.Views;
using Xamarin.Forms;

namespace QuizPlus.ViewModels
{
    public class MainMenuViewModel
    {
        public ICommand WholeWorldButtonCommand { get; }
        public ICommand EuropeButtonCommand { get; }
        public ICommand AsiaButtonCommand { get; }
        public ICommand AmericaButtonCommand { get; }
        public ICommand AfricaButtonCommand { get; }

        public MainMenuViewModel()
        {
            WholeWorldButtonCommand = new Command(HandleWholeWorldButtonCommand);
            EuropeButtonCommand = new Command(HandleEuropeButtonCommand);
            AsiaButtonCommand = new Command(HandleAsiaButtonCommand);
            AmericaButtonCommand = new Command(HandleAmericaButtonCommand);
            AfricaButtonCommand = new Command(HandleAfricaButtonCommand);
        }
        public async void HandleWholeWorldButtonCommand()
        {
            var allCountries = App.Countries;
            MainViewModel._countries = allCountries;
            await PushAsync(new MainPage());
        }        
        public async void HandleEuropeButtonCommand()
        {
            var europeCountries = App.Countries.Where(country => country.Category == Category.Europe).ToList();
            MainViewModel._countries = europeCountries;
            await PushAsync(new MainPage());
        }        
        
        public async void HandleAsiaButtonCommand()
        {
            var asiaCountries = App.Countries.Where(country => country.Category == Category.Asia).ToList();
            MainViewModel._countries = asiaCountries;
            await PushAsync(new MainPage());
        }        
        public async void HandleAmericaButtonCommand()
        {
            var americaCountries = App.Countries.Where(country => country.Category == Category.America).ToList();
            MainViewModel._countries = americaCountries;
            await PushAsync(new MainPage());
        }        
        public async void HandleAfricaButtonCommand()  
        {
            var africaCountries = App.Countries.Where(country => country.Category == Category.Africa).ToList();
            MainViewModel._countries = africaCountries;
            await PushAsync(new MainPage());
        }

        private Task PushAsync(Page page)
        {
            var navigation = Application.Current.MainPage.Navigation;
            return navigation.PushAsync(page);
        }
    }
}
