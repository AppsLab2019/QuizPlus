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
        private List<string> Countries = new List<string>()
        {
            "armensko","grecko","holandsko","lotssko","slovensko","maďarsko","ukrajina","poľsko"
        };
        private Button[] Buttons;
        private int buttonText;

        public MainPage()
        {
            InitializeComponent();
            Buttons = new[] { b1, b2, b3, b4 };
            GetRightContry();
        }

        private void Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new Page1());
            GetRightContry();
        }

        private void GetRightContry()
        {
            var random = new Random();
            var i = random.Next(0, Countries.Count - 1);
            var country = Countries[i];
            countryname.Text = country;
            countryflag.Source = $"{country}.png";
            buttonText = random.Next(0, 3);
            Buttons[buttonText].Text = country;

        }
    }
}