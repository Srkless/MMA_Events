using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMA_Fights.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentTheme = Application.Current.Resources.MergedDictionaries[0].Source.OriginalString;

            // Ako je trenutna tema svetla, prebacimo na tamnu
            if (currentTheme.Contains("/Styles/Style1.xaml"))
            {
                var darkTheme = new ResourceDictionary
                {
                    Source = new Uri("/Styles/Style2.xaml", UriKind.Relative)
                };
                Application.Current.Resources.MergedDictionaries[0] = darkTheme;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var currentTheme = Application.Current.Resources.MergedDictionaries[0].Source.OriginalString;

            // Ako je trenutna tema svetla, prebacimo na tamnu
            if (currentTheme.Contains("/Styles/Style2.xaml"))
            {
                var lightTheme = new ResourceDictionary
                {
                    Source = new Uri("/Styles/Style1.xaml", UriKind.Relative)
                };
                Application.Current.Resources.MergedDictionaries[0] = lightTheme;
            }
        }
    }
}
