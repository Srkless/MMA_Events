using MMA_Events.View;
using MMA_Events.Model;
using MMA_Events.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginView : Window
    {

        private readonly AuthService _authService = AuthService.getInstance();
        private readonly ValidationService _validationService = new ValidationService();
        public LoginView()
        {
            InitializeComponent();

            if (userEmail.IsFocused)
            {
                userEmail.Text = "";
            }
            CountryService.GetInstance();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = userEmail.Text;
            string password = userPass.Password;


            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                CustomMessageBox.Show(Application.Current.Resources["LoginFieldsError"] as string);
                return;
            }

            if (email.Contains("@"))
            {
                User user = _authService.GetUserByEmail(email);
                if (user != null && user.Password == _validationService.HashPassword(password))
                {
                    UserView userView = new UserView(user);
                    if (this.WindowState == WindowState.Maximized)
                    {
                        userView.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        userView.Width = this.Width;
                        userView.Height = this.Height;
                        userView.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    }
                    userView.Show();
                    userView.paddingAdjustment();
                    this.Close();
                }
                else
                {
                    CustomMessageBox.Show(Application.Current.Resources["LoginIvalidUserError"] as string);
                    userEmail.Text = "";
                    userPass.Password = "";
                }
            }
            else
            {
                Organizator organizator = _authService.GetByName(email);
                if (organizator != null && organizator.Password == password)
                {
                    OrganizatorView Window = new OrganizatorView(organizator);



                    if (this.WindowState == WindowState.Maximized)
                    {
                        Window.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        Window.Width = this.Width;
                        Window.Height = this.Height;
                        Window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    }
                    Window.Show();
                    Window.paddingAdjustment();
                    this.Close();
                }
                else
                {
                    CustomMessageBox.Show(Application.Current.Resources["LoginIvalidOrgError"] as string);
                    userEmail.Text = "";
                    userPass.Password = "";
                }
            }
           
            
           
        }
        private void userPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PassPromptTextBlock.Visibility = string.IsNullOrEmpty(userPass.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        // Povlačenje prozora
        private void DragBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // Promjena veličine prozora
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double newWidth = Width + e.HorizontalChange;
            double newHeight = Height + e.VerticalChange;

            if (newWidth > MinWidth)
                Width = newWidth;
            if (newHeight > MinHeight)
                Height = newHeight;
        }

        // Dodajte ovu metodu za rješavanje greške CS1061
        private void DragBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // Dodajte ovu metodu za rješavanje greške CS1061
        private void DragBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Ovdje možete dodati dodatnu logiku ako je potrebno
        }

        private void userEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // Metoda za prelazak u fullscreen
        public void FullscreenButton_Click(object sender, RoutedEventArgs e)
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
            if (WindowState != WindowState.Maximized)
            {

                userEmail.Padding = new Thickness(30, 0, 0, 2);
                userPass.Padding = new Thickness(30, 0, 0, 2);
                userEmail.FontSize = 12;
                userPass.FontSize = 12;
            }
            else
            {
                userEmail.Padding = new Thickness(50, 0, 0, 2);
                userEmail.FontSize = 20;
                userPass.Padding = new Thickness(50, 0, 0, 2);
                userPass.FontSize = 20;
            }

            EmailPromptTextBlock.FontSize = userEmail.FontSize;
            EmailPromptTextBlock.Padding = userEmail.Padding;

            PassPromptTextBlock.FontSize = userPass.FontSize;
            PassPromptTextBlock.Padding = userPass.Padding;

        }

        // Metoda za izlazak iz aplikacije
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Zatvori aplikaciju
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegisterView registerWindow = new RegisterView();
            

            if (this.WindowState == WindowState.Maximized)
            {
                registerWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                registerWindow.Width = this.Width;
                registerWindow.Height = this.Height;

                registerWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            }
            registerWindow.Show();
            registerWindow.paddingAdjustment();
            this.Close();
        }
    }
}
