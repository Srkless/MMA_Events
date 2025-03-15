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

        private void SwitchToBlackTheme(object sender, RoutedEventArgs e)
        {

            var BlackTheme = new ResourceDictionary
            {
                Source = new Uri("/Styles/Style2.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries[0] = BlackTheme;
        }

        private void SwitchToRedTheme(object sender, RoutedEventArgs e)
        {
            var RedTheme = new ResourceDictionary
            {
                Source = new Uri("/Styles/Style1.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries[0] = RedTheme;
        }

        private void SwitchToBlueRedTheme(object sender, RoutedEventArgs e)
        {
            var BlueRedTheme = new ResourceDictionary
            {
                Source = new Uri("/Styles/Style3.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries[0] = BlueRedTheme;
        }
    }
}
