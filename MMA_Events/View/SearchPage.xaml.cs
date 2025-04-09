using MMA_Events.Model;
using MMA_Events.Services;
using MMA_Events.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    /// 
    public class TemplateSelector : DataTemplateSelector
    {
        public DataTemplate FighterTemplate { get; set; }
        public DataTemplate EventTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SearchDetails details)
            {
                if (details.Fighter != null)
                    return FighterTemplate;  // Koristi template za fightera
                if (details.Event != null)
                    return EventTemplate;  // Koristi template za event
            }
            return base.SelectTemplate(item, container);
        }
    }
    public partial class SearchPage : Page
    {
        public ObservableCollection<SearchDetails> SearchDetails { get; set; }
        public FighterService FighterService = FighterService.getInstance();
        public OrganizatorView organizatorView { get; set; }
        public SearchPage()
        {
            SearchDetails = new ObservableCollection<SearchDetails>();
            InitializeComponent();
            DataContext = this;
        }

        public SearchPage(OrganizatorView view)
        {
            InitializeComponent();

            SearchDetails = new ObservableCollection<SearchDetails>();
            organizatorView = view;

            DataContext = this;
        }

        public void Search(string searchText)
        {
            SearchDetails.Clear();
            SearchService SearchService = SearchService.getInstance();

            foreach (SearchDetails details in SearchService.SearchFightersAndEvents(searchText))
            {
                SearchDetails.Add(details);
            }
        }

        public void SearchFighters(string searchText)
        {
            SearchDetails.Clear();
            SearchService SearchService = SearchService.getInstance();

            foreach(SearchDetails details in SearchService.SearchFighters(searchText))
            {
                SearchDetails.Add(details);
            }
        }

        public void SearchEvents(string searchText)
        {
            SearchDetails.Clear();
            SearchService SearchService = SearchService.getInstance();

            foreach (SearchDetails details in SearchService.SearchEvents(searchText))
            {
                SearchDetails.Add(details);
            }
        }

        private void OpenPage(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.CommandParameter is SearchDetails selectedItem)

                {
                    BaseWindow view = Window.GetWindow(this) as BaseWindow;

                    if (view == null) return;

                    if (selectedItem.Fighter == null)
                    {

                        view.bBack.Visibility = Visibility.Visible;

                        ShowFightCard showFightCard;

                        if(view is OrganizatorView orgView)
                        {
                            showFightCard = new ShowFightCard(orgView, selectedItem.Event);
                        }
                        else
                        {
                            OrganizationService orgService = OrganizationService.GetInstance();
                            Organizator org = orgService.GetOrganizatorByID(selectedItem.Event.idOrganization);
                            showFightCard = new ShowFightCard(view, selectedItem.Event, org);
                        }

                        if (view.WindowState == WindowState.Maximized)
                        {
                            showFightCard.WindowState = WindowState.Maximized;
                        }
                        else
                        {
                            showFightCard.Width = view.Width;
                            showFightCard.Height = view.Height;

                            showFightCard.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                        }
                        showFightCard.Show();
                        //registerWindow.paddingAdjustment();
                        view.Visibility = Visibility.Collapsed;
                    }
                    else if (selectedItem.Event == null)
                    {

                        view.bBack.Visibility = Visibility.Visible;
                        view.MainFrame.Content = new FighterProfilePage(selectedItem.Fighter);

                    }
                }

            }
        }

    }
}
