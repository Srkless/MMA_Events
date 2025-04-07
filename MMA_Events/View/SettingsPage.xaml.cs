using System;
using System.Collections.Generic;
using System.Globalization;
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

using System.Globalization;
using System.Threading;
using System.Windows;
using MMA_Events.Model;
using System.Collections;
using MMA_Events.Services;

namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public User user { get; set; } = null;

        public Organizator org { get; set; } = null;
        public SettingsPage(BaseWindow baseWindow, bool load = true)
        {
            if(load)
            {
                InitializeComponent();
            }
            if(baseWindow is UserView)
            {
                user = ((UserView)baseWindow).user;
                ChangeLanguage(user.Language ?? "en");
                ChangeStyle(user.Style ?? "Style1.xaml");
            }
            else if (baseWindow is OrganizatorView)
            {
                org = ((OrganizatorView)baseWindow).org;
                ChangeLanguage(org.Language ?? "en");
                ChangeStyle(org.Style ?? "Style1.xaml");
            }
        }

        private void SwitchToRedTheme(object sender, RoutedEventArgs e)
        {
            ChangeStyle("Style1.xaml");
        }

        private void SwitchToBlackTheme(object sender, RoutedEventArgs e)
        {

            ChangeStyle("Style2.xaml");
        }

        private void SwitchToBlueRedTheme(object sender, RoutedEventArgs e)
        {
            ChangeStyle("Style3.xaml");
        }

        public void ChangeStyle(string style)
        {
            ResourceDictionary theme = new ResourceDictionary();
            theme.Source = new Uri("/Styles/" + style, UriKind.Relative);

            var oldTheme = Application.Current.Resources.MergedDictionaries
              .FirstOrDefault(d => d.Contains("color1"));

            if (oldTheme != null)
            {
                int index = Application.Current.Resources.MergedDictionaries.IndexOf(oldTheme);
                Application.Current.Resources.MergedDictionaries.RemoveAt(index);
                Application.Current.Resources.MergedDictionaries.Insert(index, theme);
            }
            else
            {
                // Ako ne postoji, samo dodaj
                Application.Current.Resources.MergedDictionaries.Add(theme);
            }

            if (user != null)
            {
                user.Style = style;
                StyleService.GetInstance().UpdateUserStyleAndLanguage(user);
            }
            else if (org != null)
            {
                org.Style = style;
                StyleService.GetInstance().UpdateOrganizatorStyleAndLanguage(org);
            }

        }
        public void ChangeLanguage(string cultureCode)
        {
            ResourceDictionary dictionary = new ResourceDictionary();

            switch (cultureCode)
            {
                case "en":
                    dictionary.Source = new Uri("/Languages/Language.en.xaml", UriKind.Relative);
                    break;
                case "sr":
                    dictionary.Source = new Uri("/Languages/Language.rs.xaml", UriKind.Relative);
                    break;
            }
            
            var oldDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Contains("SettingsLangName"));

            if (oldDict != null)
            {
                int index = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                Application.Current.Resources.MergedDictionaries.RemoveAt(index);
                Application.Current.Resources.MergedDictionaries.Insert(index, dictionary);
            }
            else
            {
                // Ako ne postoji, samo dodaj
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
            }


            if (user != null)
            {
                user.Language = cultureCode;
                StyleService.GetInstance().UpdateUserStyleAndLanguage(user);
            }
            else if (org != null)
            {
                org.Language = cultureCode;
                StyleService.GetInstance().UpdateOrganizatorStyleAndLanguage(org);
            }
        }

        private void SwitchToRSLanguage(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("sr");
        }
        private void SwitchToENLanguage(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("en");
        }

    }
}
