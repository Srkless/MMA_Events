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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MMA_Events.View;
using MMA_Events.Model;

namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for OrganizatorView.xaml
    /// </summary>
    /// 

    public partial class OrganizatorView : BaseWindow
    {
        public Organizator org { get; set; }
        public override Frame MainFrame => Main;
        public override Button bBack => BackButton;

        public OrganizatorView() : base("Organizator")
        {
            InitializeComponent();
        }

        public OrganizatorView(Organizator o) : base("Organizator")
        {
            InitializeComponent();

            org = o;
            DataContext = this;

            Main.Content = new ShowEventsPage(this);
            SettingsPage settingsPage = new SettingsPage(this, false);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = e.NewSize.Width;
            Application.Current.Resources["ValueFontSize"] = width * 0.018;
            Application.Current.Resources["TypeFontSize"] = width * 0.022;
            Application.Current.Resources["FinishNumFontSize"] = width * 0.08;
            Application.Current.Resources["NameFontSize"] = width * 0.028;
            Application.Current.Resources["weightTextboxWidth"] = width * 0.040;
            Application.Current.Resources["eventCardSize"] = width * 0.25;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResize;
                paddingAdjustment();
                Application.Current.Resources["addEventFighterCardSize"] = 190;
            }
            else
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
                ResizeMode = ResizeMode.NoResize;
                paddingAdjustment();
                Application.Current.Resources["addEventFighterCardSize"] = 145;
            }

            WindowStartupLocation = WindowStartupLocation.CenterScreen;


        }


        public void paddingAdjustment()
        {
            double iconSize;
            double fontSize;
            double radioBoxSize;
            //double valueFontSize;
            //double typeFontSize;
            if (WindowState != WindowState.Maximized)
            {
                iconSize = 22;
                fontSize = 13.5;
                radioBoxSize = 50;
                //typeFontSize = 20;
                //valueFontSize = 14;

            }
            else
            {
                iconSize = 45;
                fontSize = 20;
                radioBoxSize = 80;
                //typeFontSize = 30;
                //valueFontSize = 20;
            }
            Application.Current.Resources["MenuButtonFontSize"] = fontSize;
            Application.Current.Resources["MenuIconSize"] = iconSize;
            Application.Current.Resources["RadioBoxSize"] = radioBoxSize;

            //Application.Current.Resources["ValueFontSize"] = valueFontSize;
            //Application.Current.Resources["TypeFontSize"] = typeFontSize;

        }

        // Metoda za izlazak iz aplikacije
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Zatvori aplikaciju
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void settingsRB_checked(object sender, RoutedEventArgs e)
        {
            Main.Content = new SettingsPage(this);
        }

        private void SearchRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            searchTextBlock.Visibility = Visibility.Collapsed; // Sakrij TextBlock
            searchTextBox.Visibility = Visibility.Visible;     // Prikaži TextBox
            searchTextBox.Focus();                             // Fokusiraj TextBox
        }

        private void SearchRadioButton_UnChecked(object sender, RoutedEventArgs e)
        {
            searchTextBlock.Visibility = Visibility.Visible; // Sakrij TextBlock
            searchTextBox.Visibility = Visibility.Collapsed;     // Prikaži TextBox
            searchTextBox.Text = "";
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchText = searchTextBox.Text;

                if (Main.Content is SearchPage searchPage)
                {
                    searchPage.Search(searchText);
                }
                else
                {
                    searchPage = new SearchPage();
                    Main.Content = searchPage;
                    searchPage.Search(searchText);
                }

                // Postavljanje fiksnog dela teksta pre nove pretrage
                if (searchPage.SearchDetails.Count > 0)
                    searchPage.searchLabel.Text = (Application.Current.Resources["SearchResults"] as string) + " \"" + searchText + "\"";
                else
                    searchPage.searchLabel.Text = (Application.Current.Resources["NoSearchResults"] as string) + " \"" + searchText + "\"";

                searchTextBox.Text = "";
                SearchRadioButton_UnChecked(sender, e);
                SerachRB.IsChecked = false;
            }
        }
        private void addEvent(object sender, RoutedEventArgs e)
        {
            //NavBar.Visibility = Visibility.Collapsed;
            AddEventView addEventWindow = new AddEventView(this);


            if (this.WindowState == WindowState.Maximized)
            {
                addEventWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                addEventWindow.Width = this.Width;
                addEventWindow.Height = this.Height;

                addEventWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            }
            addEventWindow.Show();
            //registerWindow.paddingAdjustment();
            this.Visibility = Visibility.Collapsed;
        }

        private void showEvents(object sender, RoutedEventArgs e)
        {
            Main.Content = new ShowEventsPage(this);
        }

        private void showFighters(object sender, RoutedEventArgs e)
        {
            Main.Content = new ShowFightersPage(this);
        }

        private void addFighter(object sender, RoutedEventArgs e)
        {
            Main.Content = new AddFighterPage(org);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
                BackButton.Visibility = MainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void rbFighters_checked(object sender, RoutedEventArgs e)
        {
            if (rbAddFighter.IsChecked == true)
            {
                addFighter(sender, e);
            }
            else if (rbShowFighters.IsChecked == true)
            {
                showFighters(sender, e);
            }
        }

        private void rbEvents_checked(object sender, RoutedEventArgs e)
        {
            if (rbAddEvent.IsChecked == true)
            {
                addFighter(sender, e);
            }
            else if (rbShowEvents.IsChecked == true)
            {
                showFighters(sender, e);
            }
        }

        private void logoutRB_checked(object sender, RoutedEventArgs e)
        {
            LoginView loginWindow = new LoginView();
            org = null;

            if (this.WindowState == WindowState.Maximized)
            {
                loginWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                loginWindow.Width = this.Width;
                loginWindow.Height = this.Height;
                loginWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            }
            loginWindow.Show();
            loginWindow.paddingAdjustment();
            this.Close();
        }

    }


}
