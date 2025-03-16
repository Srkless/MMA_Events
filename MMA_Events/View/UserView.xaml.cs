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
        public override Button bButton => BackButton;
        public UserView()
        {
            InitializeComponent();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = e.NewSize.Width;
            Application.Current.Resources["ValueFontSize"] = width * 0.018;
            Application.Current.Resources["TypeFontSize"] = width * 0.022;
            Application.Current.Resources["FinishNumFontSize"] = width * 0.08;
            Application.Current.Resources["NameFontSize"] = width * 0.028;
            Application.Current.Resources["weightTextboxWidth"] = width * 0.040;
            Application.Current.Resources["eventCardSize"] = width * 0.35;
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
            Main.Content = new ShowFightersPage();
            BackButton.Visibility = Visibility.Collapsed;
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
                MessageBox.Show($"Prikazujem rezultate za: {searchText}");
                if (Main.Content is SearchPage searchPage)
                {
                    // Pozovi metod za pretragu na postojećoj stranici
                    searchPage.Search(searchText);
                }
                else
                {
                    // Kreiraj novu instancu SearchPage i postavi je u Main.Content
                    searchPage = new SearchPage();
                    Main.Content = searchPage;
                    searchPage.Search(searchText); // Pokreni pretragu odmah
                }
                searchPage.searchLabel.Text += "\"" + searchText + "\"";
                searchTextBox.Text = "";
                SearchRadioButton_UnChecked(sender, e);
                SerachRB.IsChecked = false;
            }
        }
        private void rbDashBoard_checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbOrganizations_checked(object sender, RoutedEventArgs e)
        {

        }




        private void settingsRB_checked(object sender, RoutedEventArgs e)
        {
            Main.Content = new SettingsPage();
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

    }
}
