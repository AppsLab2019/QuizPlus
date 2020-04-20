using Xamarin.Forms;

namespace QuizPlus.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string Capital { get; set; }
        public ImageSource Image => ImageSource.FromFile($"{Name}.png");
        
        public Country(string name, string capital)
        {
            Name = name;
            Capital = capital;
        } 
    }
}
