using MMA_Events.Model;
using MMA_Events.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for ShowActiveEvents.xaml
    /// </summary>
    public partial class ShowActiveEvents : Page
    {
        public ObservableCollection<EventDetails> EventDetails { get; set; }
        public EventService service = EventService.getInstance();
        public BaseWindow baseView { get; set; }
        public ShowActiveEvents(BaseWindow baseWin)
        {
            InitializeComponent();

            EventDetails = new ObservableCollection<EventDetails>();
            baseView = baseWin;

            OrganizationService orgService = OrganizationService.GetInstance();
            List<Organizator> organizators = orgService.GetAllOrganizators();
            SubscribeToOrgService subService = SubscribeToOrgService.GetInstance();

            User user = ((UserView)baseView).user;
            foreach (Organizator org in organizators)
            {
                if (subService.IsSubscribed(user.IdUser, org.IdOrganizator))
                    foreach (EventDetails details in service.GetActiveEventsByOrganization(org))
                    {
                        EventDetails.Add(details);
                    }
            }
            DataContext = this;
        }
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;

                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var gridPanel = FindVisualChild<UniformGrid>(FightersListView);
            if (gridPanel != null)
            {
                double width = e.NewSize.Width;
                if (width > 1000)
                {
                    gridPanel.Columns = 4;

                }
                else if (width > 800)
                {
                    gridPanel.Columns = 3;
                }

                else if (width > 600)
                {
                    gridPanel.Columns = 2;

                }
                else
                {
                    gridPanel.Columns = 1;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.CommandParameter is EventDetails details)
                {

                    baseView.bBack.Visibility = Visibility.Visible;
                    UserView userView = (UserView)baseView;

                    Organizator org = OrganizationService.GetInstance().GetOrganizatorByID(details.idOrganization);
                    ShowFightCard showFightCard  = new ShowFightCard(userView, details, org);

                    if (baseView.WindowState == WindowState.Maximized)
                    {
                        showFightCard.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        showFightCard.Width = baseView.Width;
                        showFightCard.Height = baseView.Height;

                        showFightCard.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    }
                    showFightCard.Show();
                    //registerWindow.paddingAdjustment();
                    baseView.Visibility = Visibility.Collapsed;
                }

            }
        }
    }
}
