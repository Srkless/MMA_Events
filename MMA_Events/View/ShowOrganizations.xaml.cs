using FontAwesome.Sharp;
using Microsoft.VisualBasic.ApplicationServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for ShowOrganizations.xaml
    /// </summary>
    public partial class ShowOrganizations : Page
    {

        public List<Organizator> Organizators { get; set; }

        private UserView userView;
        public ShowOrganizations(UserView userView)
        {
            InitializeComponent();
            this.userView = userView;
            Organizators = OrganizationService.GetInstance().GetAllOrganizators();
            DataContext = this;
            
        }


        public ShowOrganizations(List<Organizator> organizators)
        {

            Organizators = organizators;
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Koristimo Dispatcher da obavimo rad sa UI-om u sledećem ciklusu događaja
            Dispatcher.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < OrganizationsListView.Items.Count; i++)
                {
                    // Pronađi kontejner za trenutni item
                    var item = OrganizationsListView.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                    if (item != null)
                    {
                        // Pronađi glavni dugme unutar DataTemplate-a (bOrg)
                        var mainButton = FindButtonInItem(item);
                        if (mainButton != null)
                        {
                                // Pronađi IconImage unutar dugmeta
                                var icon = FindChild<FontAwesome.Sharp.IconImage>(mainButton);
                                if (icon != null)
                                {
                                    // Uzimamo organizatora iz trenutnog itema
                                    if (OrganizationsListView.Items[i] is Organizator organizator)
                                    {
                                        int idUser = userView.user.IdUser;
                                        int idOrg = organizator.IdOrganizator;
                                        SubscribeToOrgService subscribeToOrgService = SubscribeToOrgService.GetInstance();

                                        // Ažuriraj ikonu na osnovu pretplate
                                        icon.Icon = subscribeToOrgService.IsSubscribed(idUser, idOrg)
                                            ? FontAwesome.Sharp.IconChar.Check
                                            : FontAwesome.Sharp.IconChar.Plus;
                                    }
                                }
                            
                        }
                    }
                }
            }), System.Windows.Threading.DispatcherPriority.ContextIdle);
        }

        private T FindChild<T>(DependencyObject parent, string childName = null) where T : DependencyObject
        {
            if (parent == null) return null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // Proveri da li je dete traženog tipa
                if (child is T foundChild)
                {
                    if (string.IsNullOrEmpty(childName)) return foundChild;

                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                        return foundChild;
                }

                // Rekurzivno pretraži u detetu
                var result = FindChild<T>(child, childName);
                if (result != null) return result;
            }

            return null;
        }

        private Button FindButtonInItem(FrameworkElement item)
        {
            // Prolazak kroz sve child elemente stavke da bismo našli dugme
            foreach (var child in GetAllChildren(item))
            {
                if (child is Button button && button.Name == "bSub")
                {
                    return button;
                }
            }
            return null;
        }

        private IEnumerable<DependencyObject> GetAllChildren(DependencyObject parent)
        {
            // Pomoćna metoda za pronalaženje svih child elemenata u stavci
            var children = new List<DependencyObject>();
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                children.Add(child);
                children.AddRange(GetAllChildren(child)); // Rekurzivno dodavanje child-e
            }
            return children;
        }
        private List<Button> GetButtonsFromItemsControl()
        {
            List<Button> buttons = new List<Button>();

            if (OrganizationsListView != null && OrganizationsListView.Items.Count > 0)
            {
                // Iteriranje kroz sve stavke u ItemsControl-u
                for (int i = 0; i < OrganizationsListView.Items.Count; i++)
                {
                    var item = OrganizationsListView.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                    if (item != null)
                    {
                        // Pronađi dugme unutar stavke
                        var button = item.FindName("bSub") as Button;
                        if (button != null)
                        {
                            buttons.Add(button);
                        }
                    }
                }
            }
            return buttons;
        }
        private void ShowEvents(object sender, RoutedEventArgs e)
        {

            if (sender is Button button)
            {
                if (button.CommandParameter is Organizator org)
                {

                    //OrganizatorView organizatorView = new OrganizatorView(org);
                    ShowEventsPage showEventsPage = new ShowEventsPage(userView, org);
                    EventsFightersPage eventsFightersPage = new EventsFightersPage(userView, org);
                    userView.MainFrame.Content = eventsFightersPage;
                    
                }
            }
        }
        

        private void AddToFavourite(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            
             Button innerButton = sender as Button;
            if (innerButton == null) return;

            // Pronađi roditeljsko dugme (prvo dugme)
            DependencyObject parent = VisualTreeHelper.GetParent(innerButton);
            while (parent != null && !(parent is Button))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is Button outerButton)
            {
                Organizator selectedOrg = (Organizator)outerButton.CommandParameter;
                int idUser = userView.user.IdUser;
                int idOrg = selectedOrg.IdOrganizator;
                SubscribeToOrgService subscribeToOrgService = SubscribeToOrgService.GetInstance();
                

                if(subscribeToOrgService.IsSubscribed(idUser, idOrg))
                {
                    if (subscribeToOrgService.unsubscribe(idUser, idOrg))
                    {
                        FontAwesome.Sharp.IconImage icon = (FontAwesome.Sharp.IconImage)innerButton.Content;
                        icon.Icon = FontAwesome.Sharp.IconChar.Plus;
                    }
                }
                else
                {
                    if (subscribeToOrgService.subscribe(idUser, idOrg))
                    {
                        FontAwesome.Sharp.IconImage icon = (FontAwesome.Sharp.IconImage)innerButton.Content;
                        icon.Icon = FontAwesome.Sharp.IconChar.Check;
                    }
                }
            }

        }

    }
}
