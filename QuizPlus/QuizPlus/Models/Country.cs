using System;
using System.Collections.Generic;
using System.Text;

namespace QuizPlus.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string Capital { get; set; }
        
        public Country(string name, string capital)
        {
            Name = name;
            Capital = capital;
        }
        public Country()
        {

        }
    }
}
