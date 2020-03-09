using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuizPlus
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        
        private Country CorrectCountry;
        private List<Country> Countries = new List<Country>()
        {
            new Country("Armenia", "Jerevan"), new Country("Greece", "Athens"), new Country("Hungary", "Budapest"), new Country("Latvia", "Riga"), new Country("Netherlands", "Amsterdam"), new Country("Poland", "Warsaw"), new Country("Slovakia", "Bratislava"), new Country("Ukraine", "Kiev")
        };
        private Random rnd = new Random();        
        private Button[] Buttons;
        public MainPage()
        {                        
            InitializeComponent();
            Buttons = new[] { b1, b2, b3, b4 };
            CorrectCountry = Countries[rnd.Next(0, Countries.Count)];
            label.Text = CorrectCountry.Name;
            foreach(var btn in Buttons)
            {
                btn.Text = Countries[rnd.Next(0, Countries.Count)].Capital;
            }
        }

        private void Clicked(object sender, EventArgs e)
        {
            
        }

        
    }
}