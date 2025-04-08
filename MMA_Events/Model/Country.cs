using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MMA_Events.Model
{
    class Country
    {
        public string Name { get; set; }
        public ImageSource FlagImage { get; set; }

        public Country(string name)
        {
            Name = name;
            LoadFlagImage();
        }

        private void LoadFlagImage()
        {
            try
            {
                // Priprema ispravnog URI-ja
                string escapedName = Uri.EscapeDataString(Name);
                string uriString = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Countries", $"{Name}", "flag.png");
                // Pokušaj učitavanja sa originalnim imenom
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(uriString, UriKind.Relative);
                img.CacheOption = BitmapCacheOption.OnLoad; // Da izbjegnemo probleme sa zaključavanjem fajlova
                img.EndInit();
                FlagImage = img;
            }
            catch
            {
                // Ako ne možemo učitati zastavu, koristimo default zastavu ili praznu sliku
                try
                {
                    // Alternativno možete koristiti praznu zastavu ili placeholder
                    FlagImage = new BitmapImage(new Uri("/Images/logo.png", UriKind.Relative));
                }
                catch
                {
                    // Ako ništa ne uspije, ostavljamo null
                    FlagImage = null;
                }
            }
        }

        // Za debugging
        public override string ToString()
        {
            return Name;
        }
    }
}
