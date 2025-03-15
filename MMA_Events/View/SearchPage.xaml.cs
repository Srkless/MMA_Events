using MMA_Events.Model;
using MMA_Events.Services;
using MMA_Fights;
using MMA_Fights.Model;
using MMA_Fights.Services;
using MMA_Fights.View;
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
            InitializeComponent();
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

            if (organizatorView != null)
            {
                foreach (SearchDetails details in SearchService.SearchFightersAndEvents(searchText))
                {
                    SearchDetails.Add(details);
                }
            }
        }

        private void OpenPage(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.CommandParameter is SearchDetails selectedItem)
                {
                    if (selectedItem.Fighter == null)
                    {
                        
                        organizatorView.BackButton.Visibility = Visibility.Visible;

                        ShowFightCard showFightCard = new ShowFightCard(organizatorView, selectedItem.Event);

                        if (organizatorView.WindowState == WindowState.Maximized)
                        {
                            showFightCard.WindowState = WindowState.Maximized;
                        }
                        else
                        {
                            showFightCard.Width = organizatorView.Width;
                            showFightCard.Height = organizatorView.Height;

                            showFightCard.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                        }
                        showFightCard.Show();
                        //registerWindow.paddingAdjustment();
                        organizatorView.Visibility = Visibility.Collapsed;
                    }
                    else if (selectedItem.Event == null)
                    {

                        organizatorView.BackButton.Visibility = Visibility.Visible;
                        organizatorView.Main.Content = new FighterProfilePage(selectedItem.Fighter);

                    }
                }
                    
            }
        }

    }
}
