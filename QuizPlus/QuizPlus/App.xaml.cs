using QuizPlus.Models;
using QuizPlus.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace QuizPlus
{
    public partial class App
    {
        public static List<Country> Countries { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new Page();
        }

        protected override async void OnStart()
        {
            var database = new Database();

            await database.Initialize();
            Countries = await database.GetAllCountries();

            MainPage = new NavigationPage(new MainMenu());
        }
    }
}
