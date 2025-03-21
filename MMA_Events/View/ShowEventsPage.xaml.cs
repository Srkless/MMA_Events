﻿using MMA_Events.Model;
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
    /// Interaction logic for ShowEventsPage.xaml
    /// </summary>
    /// 
    public partial class ShowEventsPage : Page
    {
        public ObservableCollection<EventDetails> EventDetails { get; set; }
        public EventService service = EventService.getInstance();
        public OrganizatorView organizatorView { get; set; }
        public ShowEventsPage()
        {
            InitializeComponent();
            EventDetails = new ObservableCollection<EventDetails>();


            DataContext = this;
        }

        public ShowEventsPage(OrganizatorView view)
        {
            InitializeComponent();

            EventDetails = new ObservableCollection<EventDetails>();
            organizatorView = view;

            if (view != null)
            {
                Organizator organizator = view.org;
                foreach (EventDetails details in service.GetAllEventsByOrganization(organizator))
                {
                    EventDetails.Add(details);
                }
            }

            DataContext = this;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button button)
            {
                if (button.CommandParameter is EventDetails details)
                {

                    organizatorView.BackButton.Visibility = Visibility.Visible;

                    ShowFightCard showFightCard = new ShowFightCard(organizatorView, details);

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
            }
        }

    }
}
