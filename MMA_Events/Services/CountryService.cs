using MMA_Events.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Services
{
    class CountryService
    {
        private static CountryService instance = null;
        public List<Country> countries = null;

        private CountryService()
        {
            if (countries == null)
            {
                String countriesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Countries");

                if (!Directory.Exists(countriesPath))
                    countries = new List<Country>();

                var countryFolders = Directory.GetDirectories(countriesPath);

                countries = countryFolders.Select(folderPath => new Country(Path.GetFileName(folderPath))).ToList();
            }

        }

        public static CountryService GetInstance()
        {
            if (instance == null)
            {
                instance = new CountryService();
            }
            return instance;
        }
    }
}
