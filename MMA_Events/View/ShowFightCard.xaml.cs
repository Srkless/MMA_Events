using MMA_Events.Model;
using MMA_Events.Services;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for ShowFightCard.xaml
    /// </summary>
    public partial class ShowFightCard : Window, INotifyPropertyChanged
    {

        public BaseWindow baseWindow { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public List<(int idFight, FighterDetails redCorner, FighterDetails blueCorner)> fighters;
        public EventDetails details;

        int selectedFight;

        private FighterDetails _selectedFighterLeft;
        public FighterDetails SelectedFighterLeft
        {
            get => _selectedFighterLeft;
            set
            {
                _selectedFighterLeft = value;
                OnPropertyChanged(nameof(SelectedFighterLeft));
            }
        }

        private FighterDetails _selectedFighterRight;
        public FighterDetails SelectedFighterRight
        {
            get => _selectedFighterRight;
            set
            {
                _selectedFighterRight = value;
                OnPropertyChanged(nameof(SelectedFighterRight));
            }
        }

        private FighterDetails _winner;
        public FighterDetails Winner
        {
            get => _winner;
            set
            {
                _winner = value;
                OnPropertyChanged(nameof(Winner));
            }
        }

        private int currentIndex = 0;
        private Organizator org = null;
        public ShowFightCard()
        {
            InitializeComponent();
            fighters = new List<(int idFight, FighterDetails redCornder, FighterDetails redCorder)>();
        }

        public ShowFightCard(BaseWindow baseWindow, EventDetails details, Organizator org = null)
        {
            InitializeComponent();
            this.baseWindow = baseWindow;
            this.details = details;
            this.org = org;

            EventService service = EventService.getInstance();
            if (org == null)
            {
                if (baseWindow is OrganizatorView organizatorView && organizatorView != null)
                {
                    this.org = organizatorView.org;
                }
            }
            fighters = service.GetFightersFromCard(this.org, details);

            FightService fightService = FightService.getInstance();

            SelectedFighterLeft = fighters[currentIndex].redCorner;
            SelectedFighterRight = fighters[currentIndex].blueCorner;
            selectedFight = fighters[currentIndex].idFight;

            updateResults();

            showButtons();

            eventName.Text = details.Name;
            eventVenue.Text = details.Location;
            eventDate.Text = details.Date;

            DataContext = this;
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Zatvori aplikaciju
        }

        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResize;
                fieldMethod.Margin = new Thickness(0, 0, -50, 0);
                paddingAdjustment();
            }
            else
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
                ResizeMode = ResizeMode.NoResize;
                fieldMethod.Margin = new Thickness(0, 0, -150, 0);
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

        private void OnlyNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Dozvoljava samo brojeve
            e.Handled = !int.TryParse(e.Text, out _);

        }

        private void OnlyText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Z\s]+$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true; // Blokira unos nedozvoljenih znakova
            }

        }

        private void AllowControlKeys(object sender, KeyEventArgs e)
        {
            // Dozvoljava Backspace, Delete, strelice, Tab
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab ||
                e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = false;
            }
        }
        private void btnT_Unchecked(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(btnS, Dock.Left);
            ToggleGradientDirection(btnT);

            leftFighterWinner.Visibility = Visibility.Visible;
            rightFighterWinner.Visibility = Visibility.Collapsed;
            Winner = SelectedFighterLeft;
        }
        private void btnT_Checked(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(btnS, Dock.Right);
            ToggleGradientDirection(btnT);

            leftFighterWinner.Visibility = Visibility.Collapsed;
            rightFighterWinner.Visibility = Visibility.Visible;
            Winner = SelectedFighterRight;
        }



        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void changeFight(bool direction)
        {
            if (direction == true && (currentIndex + 1 < fighters.Count))
            {
                currentIndex += 1;
            }

            if ((0 < currentIndex) && direction == false)
            {
                currentIndex -= 1;

            }
            SelectedFighterLeft = fighters[currentIndex].redCorner;
            SelectedFighterRight = fighters[currentIndex].blueCorner;
            selectedFight = fighters[currentIndex].idFight;

            updateResults();


            showButtons();

            DataContext = this;
        }

        private void updateResults()
        {
            FightService fightService = FightService.getInstance();
            Fight f = fightService.GetFightByID(selectedFight);

            if (f == null)
            {
                return;
            }

            if (baseWindow.Type == "User")
            {
                btnT.IsEnabled = false;
                //return;
            }

            if (f.winnerId == null || f.roundEnded == null || f.Method == null)
            {
                fieldRound.Text = "";
                fieldMethod.Text = "";
                fieldRound.IsEnabled = true;
                fieldMethod.IsEnabled = true;
                leftFighterWinner.Visibility = Visibility.Collapsed;
                rightFighterWinner.Visibility = Visibility.Collapsed;
                btnT.IsEnabled = true;
            }
            else
            {
                fieldRound.Text = f.roundEnded.ToString();
                fieldMethod.Text = f.Method.ToString();
                fieldRound.IsEnabled = false;
                fieldMethod.IsEnabled = false;

                if (f.winnerId == f.redCornedId)
                {
                    DockPanel.SetDock(btnS, Dock.Left);
                    ToggleGradientDirection(btnT);
                    leftFighterWinner.Visibility = Visibility.Visible;
                    rightFighterWinner.Visibility = Visibility.Collapsed;
                    btnT.IsEnabled = false;

                }
                else if (f.winnerId == f.blueCornedId)
                {
                    DockPanel.SetDock(btnS, Dock.Right);
                    ToggleGradientDirection(btnT);
                    leftFighterWinner.Visibility = Visibility.Collapsed;
                    rightFighterWinner.Visibility = Visibility.Visible;
                    btnT.IsEnabled = false;
                }
                else
                {
                    leftFighterWinner.Visibility = Visibility.Collapsed;
                    rightFighterWinner.Visibility = Visibility.Collapsed;
                    btnT.IsEnabled = true;
                    Winner = null;
                }


            }
            
        }
        private void showButtons()
        {
            if(fighters.Count == 1)
            {
                prevFight.Visibility = Visibility.Collapsed;
                nextFight.Visibility = Visibility.Collapsed;
            }
            else if (currentIndex == 0 && currentIndex < fighters.Count - 1)
            {
                prevFight.Visibility = Visibility.Collapsed;
                nextFight.Visibility = Visibility.Visible;
            }
            else if (currentIndex == fighters.Count - 1)
            {
                prevFight.Visibility = Visibility.Visible;
                nextFight.Visibility = Visibility.Collapsed;
            }
            else
            {
                prevFight.Visibility = Visibility.Visible;
                nextFight.Visibility = Visibility.Visible;
            }
        }
        private void ToggleGradientDirection(ToggleButton button)
        {
            if (button.Template != null)
            {
                button.ApplyTemplate();

                // Pronađi Border unutar ToggleButton-a
                Border border = (Border)button.Template.FindName("border", button);
                if (border != null && border.BorderBrush is LinearGradientBrush oldBrush)
                {
                    // Napravi novu kopiju Brush-a jer je originalni zamrznut
                    LinearGradientBrush newBrush = new LinearGradientBrush
                    {
                        GradientStops = new GradientStopCollection(oldBrush.GradientStops), // Kopiramo gradijentne boje
                        StartPoint = oldBrush.StartPoint,
                        EndPoint = oldBrush.EndPoint
                    };

                    // Preklopimo StartPoint i EndPoint
                    if (newBrush.StartPoint == new Point(0, 0) && newBrush.EndPoint == new Point(1, 1))
                    {
                        newBrush.StartPoint = new Point(1, 1);
                        newBrush.EndPoint = new Point(0, 0);
                    }
                    else
                    {
                        newBrush.StartPoint = new Point(0, 0);
                        newBrush.EndPoint = new Point(1, 1);
                    }

                    // Postavi novu četkicu
                    border.BorderBrush = newBrush;
                }
            }
        }



        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = e.NewSize.Width;
            Application.Current.Resources["ValueFontSize"] = width * 0.018;
            Application.Current.Resources["TypeFontSize"] = width * 0.022;
            Application.Current.Resources["FinishNumFontSize"] = width * 0.08;
            Application.Current.Resources["NameFontSize"] = width * 0.028;
            Application.Current.Resources["weightTextboxWidth"] = width * 0.040;
            Application.Current.Resources["rectangleHeight"] = width * 0.1;
            Application.Current.Resources["FinishNumFontSize"] = width * 0.065;
            Application.Current.Resources["eventResultFontSize"] = width * 0.017;
            this.Resources["scoreNameLabelLeft"] = new CornerRadius(0, width * 0.1 * 0.5, width * 0.1 * 0.5, 0);
            this.Resources["scoreNameLabelRight"] = new CornerRadius(width * 0.1 * 0.5, 0, 0, width * 0.1 * 0.5);
            this.Resources["controlButtons"] = new CornerRadius(width * 0.02);

            if (this.WindowState == WindowState.Maximized)
            {
                Application.Current.Resources["dockButtonWidth"] = 228.0;
            }
            else
            {
                Application.Current.Resources["dockButtonWidth"] = 92.0;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {

            if (this.WindowState == WindowState.Maximized)
            {
                baseWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                baseWindow.Width = this.Width;
                baseWindow.Height = this.Height;

                baseWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            }
            baseWindow.Show();
            if (baseWindow is OrganizatorView organizatorView && organizatorView != null)
                organizatorView.rbAddEvent.IsChecked = false;
            baseWindow.Visibility = Visibility.Visible;
            this.Close();

        }

        private void nextFight_Click(object sender, RoutedEventArgs e)
        {
            changeFight(true);
        }

        private void prevFight_Click(object sender, RoutedEventArgs e)
        {
            changeFight(false);
        }

        private void UpdateFightResult(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (fieldMethod.Text != "" && fieldRound.Text != "" && (leftFighterWinner.Visibility == Visibility.Visible || rightFighterWinner.Visibility == Visibility.Visible))
                {
                    if (!fieldMethod.Text.Equals(FightMethod.KO.ToString(), StringComparison.OrdinalIgnoreCase) &&
                        !fieldMethod.Text.Equals(FightMethod.Submission.ToString(), StringComparison.OrdinalIgnoreCase) &&
                        !fieldMethod.Text.Equals(FightMethod.Decision.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        CustomMessageBox.Show(Application.Current.Resources["InvalidMethodError"] as string);
                        return;
                    }
                    if (int.Parse(fieldRound.Text) < 1 || int.Parse(fieldRound.Text) > 5)
                    {
                        CustomMessageBox.Show(Application.Current.Resources["InvalidRoundNumberError"] as string);
                        return;
                    }
                    FightService fightService = FightService.getInstance();
                    fightService.UpdateFightResult(details.idFightCard, Winner.idFighter, fieldMethod.Text, int.Parse(fieldRound.Text), selectedFight);

                    fieldMethod.IsEnabled = false;
                    fieldRound.IsEnabled = false;
                }
                else
                {
                    CustomMessageBox.Show(Application.Current.Resources["UpdateFighterFieldsError"] as string);
                }
            }
        }
    }
}
