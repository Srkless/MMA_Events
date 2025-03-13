using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MMA_Fights.Services
{
    public class NicknameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string nickname = value as string;
            if (string.IsNullOrEmpty(nickname))
            {
                return string.Empty; // Ako je prazno, ne dodajemo ništa.
            }
            return $"\"{nickname}\""; // Dodajemo navodnike oko Nickname.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // Ovde možete implementirati povratnu konverziju ako je potrebno.
        }
    }
}
