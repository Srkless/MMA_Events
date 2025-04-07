using MMA_Events.Model;
using MMA_Events.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for AddEvent.xaml
    /// </summary>
    public partial class AddEventView : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<FighterDetails> FighterDetailsLeft { get; set; }
        public ObservableCollection<FighterDetails> FighterDetailsRight { get; set; }

        private FighterDetails _selectedFighterLeft;
        public FighterDetails SelectedFighterLeft
        {
            get { return _selectedFighterLeft; }
            set
            {
                _selectedFighterLeft = value;
                OnPropertyChanged(nameof(SelectedFighterLeft));
            }
        }
        private FighterDetails _selectedFighterRight;
        public FighterDetails SelectedFighterRight
        {
            get { return _selectedFighterRight; }
            set
            {
                _selectedFighterRight = value;
                OnPropertyChanged(nameof(SelectedFighterRight));
            }
        }
        public OrganizatorView organizatorView { get; set; }


        public Event Event = null;
        public FightCard card = null;
        public AddEventView()
        {
            InitializeComponent();
        }

        public AddEventView(OrganizatorView view)
        {
            InitializeComponent();

            FighterDetailsLeft = new ObservableCollection<FighterDetails>();
            FighterDetailsRight = new ObservableCollection<FighterDetails>();
            FighterService FighterService = FighterService.getInstance();
            organizatorView = view;

            if (view != null)
            {
                Organizator organizator = view.org;
                foreach (FighterDetails details in FighterService.GetAllFighters(organizator))
                {
                    FighterDetailsLeft.Add(details);
                    FighterDetailsRight.Add(details);
                }
            }

            DataContext = this;
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
            this.Resources["scoreNameLabelLeft"] = new CornerRadius(0, width * 0.1 * 0.5, width * 0.1 * 0.5, 0);
            this.Resources["scoreNameLabelRight"] = new CornerRadius(width * 0.1 * 0.5, 0, 0, width * 0.1 * 0.5);
            this.Resources["controlButtons"] = new CornerRadius(width * 0.02);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
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

        private void ChooseLeftFigther(object sender, RoutedEventArgs e)
        {

            if (sender is Button button && button.CommandParameter is FighterDetails fighter)
            {
                FightersListViewLeft.Visibility = Visibility.Collapsed;
                ShowFighterLeft.Visibility = Visibility.Visible;
                SelectedFighterLeft = fighter;
                FighterDetailsRight.Remove(fighter);
            }
        }
        private void ChooseRightFigther(object sender, RoutedEventArgs e)
        {

            if (sender is Button button && button.CommandParameter is FighterDetails fighter)
            {
                FightersListViewRight.Visibility = Visibility.Collapsed;
                ShowFighterRight.Visibility = Visibility.Visible;
                SelectedFighterRight = fighter;
                FighterDetailsLeft.Remove(fighter);
            }

        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                organizatorView.WindowState = WindowState.Maximized;
            }
            else
            {
                organizatorView.Width = this.Width;
                organizatorView.Height = this.Height;

                organizatorView.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            }
            organizatorView.Show();
            organizatorView.rbAddEvent.IsChecked = false;
            organizatorView.Visibility = Visibility.Visible;
            organizatorView.Main.Content = new ShowEventsPage(organizatorView);
            this.Close();
        }
        private void SwitchFighters_Click(object sender, RoutedEventArgs e)
        {

            FighterDetailsRight.Add(SelectedFighterLeft);
            FighterDetailsLeft.Add(SelectedFighterRight);

            FighterDetails temp = SelectedFighterLeft;
            SelectedFighterLeft = SelectedFighterRight;
            SelectedFighterRight = temp;

            FighterDetailsRight.Remove(SelectedFighterLeft);
            FighterDetailsLeft.Remove(SelectedFighterRight);
        }
        private void ChangeFighters_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFighterLeft != null)
            {
                FighterDetailsRight.Add(SelectedFighterLeft);
                SelectedFighterLeft = null;

            }
            if (SelectedFighterRight != null)
            {
                FighterDetailsLeft.Add(SelectedFighterRight);
                SelectedFighterRight = null;
            }

            FightersListViewRight.Visibility = Visibility.Visible;
            ShowFighterRight.Visibility = Visibility.Collapsed;

            FightersListViewLeft.Visibility = Visibility.Visible;
            ShowFighterLeft.Visibility = Visibility.Collapsed;

        }


        private void AcceptFight(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                EventService service = EventService.getInstance();
                if (fieldEventName.Text != "" && fieldEventDate.Text != "" && fieldEventVenue.Text != "")
                {
                    fieldEventName.Visibility = Visibility.Collapsed;
                    fieldEventDate.Visibility = Visibility.Collapsed;
                    fieldEventVenue.Visibility = Visibility.Collapsed;

                    DateTime? date_DateTime = fieldEventDate.SelectedDate;
                    string date = date_DateTime.Value.ToString("yyyy-MM-dd");

                    eventName.Text = fieldEventName.Text;
                    eventDate.Text = fieldEventDate.Text;
                    eventVenue.Text = fieldEventVenue.Text;

                    eventName.Visibility = Visibility.Visible;
                    eventDate.Visibility = Visibility.Visible;
                    eventVenue.Visibility = Visibility.Visible;


                    VenuePromptTextBlock.Visibility = Visibility.Collapsed;
                    DatePromptTextBlock.Visibility = Visibility.Collapsed;
                    NamePromptTextBlock.Visibility = Visibility.Collapsed;

                    fieldEventName.Text = "";
                    fieldEventDate.Text = "";
                    fieldEventVenue.Text = "";


                    Event = new Event
                    {
                        Name = eventName.Text,
                        Date = date,
                        Location = eventVenue.Text,
                        IdOrganization = organizatorView.org.IdOrganizator
                    };

                    if ((Event.Id = service.createEvent(Event)) == null)
                        CustomMessageBox.Show(Application.Current.Resources["EventCreationError"] as string); 
                }
                else if (Event == null)
                {
                    CustomMessageBox.Show(Application.Current.Resources["EventFieldsError"] as string);
                }

                if (SelectedFighterRight != null && SelectedFighterLeft != null && Event != null)
                {
                    if (card == null)
                    {
                        card = new FightCard
                        {
                            idEvent = Event.Id
                        };
                        card.id = service.createCard(card);
                    }
                    FightersListViewRight.Visibility = Visibility.Visible;
                    ShowFighterRight.Visibility = Visibility.Collapsed;

                    FightersListViewLeft.Visibility = Visibility.Visible;
                    ShowFighterLeft.Visibility = Visibility.Collapsed;

                    FighterDetailsLeft.Remove(SelectedFighterLeft);
                    FighterDetailsRight.Remove(SelectedFighterRight);

                    Fight fight = new Fight
                    {
                        redCornedId = SelectedFighterLeft.idFighter,
                        blueCornedId = SelectedFighterRight.idFighter,
                        idEvent = Event.Id,
                        idCard = card.id,
                    };

                    if (!service.addFight(fight))
                    {
                        CustomMessageBox.Show(Application.Current.Resources["FightCreationError"] as string);
                    }
                }
            }
        }
    }
}
