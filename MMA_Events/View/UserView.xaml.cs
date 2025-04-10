using MMA_Events.Model;
using MMA_Events.Services;
using MMA_Events.View;
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
using System.Windows.Shapes;
namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : BaseWindow
    {

        public override Frame MainFrame => Main;
        public override Button bBack => BackButton;

        public User user { get; set; }
        public UserView(User user) : base("User")
        {
            InitializeComponent();

            this.user = user;
            paddingAdjustment();
            Main.Content = new ShowActiveEvents(this);
            DashboardRB.IsChecked = true;

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
            }
            else
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
                ResizeMode = ResizeMode.NoResize;
                paddingAdjustment();
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
                BackButton.Visibility = MainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
            }
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
            //searchTextBox.Text = "";
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(searchTextBox.Text != "")
                searchIcon.Icon = FontAwesome.Sharp.IconChar.Eraser;
            else
                searchIcon.Icon = FontAwesome.Sharp.IconChar.MagnifyingGlass;
            if (e.Key == Key.Enter)
            {
                string searchText = searchTextBox.Text;
                

                

                if(rbSearchFighters.IsChecked == true)
                {
                    searchFighters();
                }

                else if (rbSearchEvents.IsChecked == true)
                {
                    searchEvents();
                }
                else
                {
                    searchAll();    
                }
                
                //searchTextBox.Text = "";
                //SearchRadioButton_UnChecked(sender, e);
                //SerachRB.IsChecked = false;
            }

        }


        public void searchAll()
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
            if (searchPage.SearchDetails.Count > 0)
                searchPage.searchLabel.Text = (Application.Current.Resources["SearchResults"] as string) + " \"" + searchText + "\"";
            else
                searchPage.searchLabel.Text = (Application.Current.Resources["NoSearchResults"] as string) + " \"" + searchText + "\"";
            if (searchTextBox.Text == "")
                searchPage.searchLabel.Text = Application.Current.Resources["AllSearchResults"] as string;
        }
        public void searchFighters()
        {
            string searchText = searchTextBox.Text;
            if (Main.Content is SearchPage searchPage)
            {
                searchPage.SearchFighters(searchText);
            }
            else
            {
                searchPage = new SearchPage();
                Main.Content = searchPage;
                searchPage.SearchFighters(searchText);
            }
            if (searchPage.SearchDetails.Count > 0)
                searchPage.searchLabel.Text = (Application.Current.Resources["SearchFightersResults"] as string) + " \"" + searchText + "\"";
            else
                searchPage.searchLabel.Text = (Application.Current.Resources["NoSearchFightersResults"] as string) + " \"" + searchText + "\"";

            if (searchTextBox.Text == "")
                searchPage.searchLabel.Text = Application.Current.Resources["AllFightersSearchResults"] as string;

        }

        public void searchEvents()
        {
            string searchText = searchTextBox.Text;
            if (Main.Content is SearchPage searchPage)
            {
                searchPage.SearchEvents(searchText);
            }
            else
            {
                searchPage = new SearchPage();
                Main.Content = searchPage;
                searchPage.SearchEvents(searchText);
            }
            if (searchPage.SearchDetails.Count > 0)
                searchPage.searchLabel.Text = (Application.Current.Resources["SearchEventsResults"] as string) + " \"" + searchText + "\"";
            else
                searchPage.searchLabel.Text = (Application.Current.Resources["NoSearchEventsResults"] as string) + " \"" + searchText + "\"";

            if (searchTextBox.Text == "")
                searchPage.searchLabel.Text = Application.Current.Resources["AllSEventSearchResults"] as string;
        }
        private void RadioButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            // Ako je već čekiran, odčekiraj ga i zaustavi normalno ponašanje
            if (rb != null && rb.IsChecked == true)
            {
                rb.IsChecked = false;
                e.Handled = true; // zaustavi da WPF ponovo čekira
                //searchTextBox.Text = "";
                searchAll();
            }
        }

        private void showFighters(object sender, RoutedEventArgs e)
        {
            //SearchTextBox_KeyDown(sender, e);
            searchFighters();
        }

        private void showEvents(object sender, RoutedEventArgs e)
        {
            searchEvents();
            
        }

        private void ClearSearchBox(object sender, RoutedEventArgs e)
        {
            if (searchIcon.Icon == FontAwesome.Sharp.IconChar.Eraser)
            {
                searchTextBox.Text = "";
                searchIcon.Icon = FontAwesome.Sharp.IconChar.MagnifyingGlass;
                rbSearchEvents.IsChecked = false;
                rbSearchFighters.IsChecked = false;
                SearchRadioButton_UnChecked(sender, e);
                SerachRB.IsChecked = false;
                searchAll();

            }
            else
            {
                searchAll();
            }
        }

        private void rbDashBoard_checked(object sender, RoutedEventArgs e)
        {
            Main.Content = new ShowActiveEvents(this);
        }

        private void rbOrganizations_checked(object sender, RoutedEventArgs e)
        {
            Main.Content = new ShowOrganizations(this);
        }

        private void settingsRB_checked(object sender, RoutedEventArgs e)
        {
            Main.Content = new SettingsPage(this);
        }

        private void logoutRB_checked(object sender, RoutedEventArgs e)
        {
            LoginView loginWindow = new LoginView();

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
        public void ReloadWindow()
        {
            this.InvalidateVisual(); // Osvežava trenutni prozor
        }

       
    }
}
