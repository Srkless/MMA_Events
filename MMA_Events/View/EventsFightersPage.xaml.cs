using MMA_Events.Model;
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

namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for EventsFightersPage.xaml
    /// </summary>
    public partial class EventsFightersPage : Page
    {

        private UserView userView;
        private Organizator org;
        public EventsFightersPage()
        {
            InitializeComponent();
        }

        public EventsFightersPage(UserView userView, Organizator org)
        {
            InitializeComponent();
            this.userView = userView;
            this.org = org;
            userView.BackButton.Visibility = Visibility.Visible;
        }

        private void showEvents(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                //OrganizatorView organizatorView = new OrganizatorView(org);
                ShowEventsPage eventsPage = new ShowEventsPage(userView, org);
                userView.MainFrame.Content = eventsPage;

            }
        }
        private void showFighters(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                ShowFightersPage fighterssPage = new ShowFightersPage(userView,org);
                userView.MainFrame.Content = fighterssPage;
            }

        }
    }
}
