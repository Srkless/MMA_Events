using MMA_Events.Model;
using MMA_Events.Services;
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
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {

        private readonly AuthService _registerService;
        private readonly ValidationService _validationService = new ValidationService();

        public RegisterView()
        {
            InitializeComponent();
            CountryComboBox.ItemsSource = CountryService.GetInstance().countries;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
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

        private void userPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PassPromptTextBlock.Visibility = string.IsNullOrEmpty(userPassField.Password) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void userConfirmPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ConfirmPassPromptTextBlock.Visibility = string.IsNullOrEmpty(userConfirmPassField.Password) ? Visibility.Visible : Visibility.Collapsed;
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
            Thickness padding;
            int fontSize;
            if (WindowState != WindowState.Maximized)
            {
                padding = new Thickness(30, 0, 0, 2);
                fontSize = 12;

            }
            else
            {
                padding = new Thickness(50, 0, 0, 2);
                fontSize = 20;
            }

            userNameField.Padding = padding;
            userNameField.FontSize = fontSize;


            userSurnameField.Padding = padding;
            userSurnameField.FontSize = fontSize;

            userEmailField.Padding = padding;
            userEmailField.FontSize = fontSize;

            //userCountryField.Padding = padding;
            //userCountryField.FontSize = fontSize;

            CountryComboBox.Padding = padding;
            CountryComboBox.FontSize = fontSize;
            countryIcon.Width = padding.Left;

            userPassField.Padding = padding;
            userPassField.FontSize = fontSize;

            userConfirmPassField.Padding = padding;
            userConfirmPassField.FontSize = fontSize;

            NamePromptTextBlock.Padding = padding;
            NamePromptTextBlock.FontSize = fontSize;

            SurnamePromptTextBlock.Padding = padding;
            SurnamePromptTextBlock.FontSize = fontSize;

            EmailPromptTextBlock.Padding = padding;
            EmailPromptTextBlock.FontSize = fontSize;

            //CountryPromptTextBlock.Padding = padding;
            //CountryPromptTextBlock.FontSize = fontSize;

            PassPromptTextBlock.Padding = padding;
            PassPromptTextBlock.FontSize = fontSize;

            ConfirmPassPromptTextBlock.Padding = padding;
            ConfirmPassPromptTextBlock.FontSize = fontSize;

        }

        // Metoda za izlazak iz aplikacije
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Zatvori aplikaciju
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = userNameField.Text;
            string surname = userSurnameField.Text;
            string email = userEmailField.Text;
            //string country = userCountryField.Text;
            string country = CountryComboBox.Text;
            string password = userPassField.Password;
            string confPass = userConfirmPassField.Password;
            string encPass;

            AuthService registerService = AuthService.getInstance();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(country) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confPass))
            {
                CustomMessageBox.Show(Application.Current.Resources["RegistrationFieldsError"] as string);
                return;
            }

            User user = null;

            if (!_validationService.validateEmail(email))
            {
                CustomMessageBox.Show(Application.Current.Resources["RegistrationInvalidEmailError"] as string);
                return;
            }

            if (_validationService.ValidatePassword(password, confPass))
            {
                encPass = _validationService.HashPassword(password);
                if (registerService.GetUserByEmail(email) == null && registerService.registerUser(username, surname, email, country, encPass))
                {
                    BtnLogin_Click(sender, e);
                }
                else
                    CustomMessageBox.Show(Application.Current.Resources["RegistrationEmailError"] as string);
            }
            else
            {
                CustomMessageBox.Show(Application.Current.Resources["RegistrationPasswordError"] as string);
                return;
            }
        }
    }
}
