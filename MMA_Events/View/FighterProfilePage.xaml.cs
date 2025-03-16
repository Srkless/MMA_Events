using Microsoft.EntityFrameworkCore;
using MMA_Events.Model;
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
    /// Interaction logic for ShowFighters.xaml
    /// </summary>
    public partial class FighterProfilePage : Page
    {

        public FighterDetails FighterDetails { get; set; }

        public FighterProfilePage(FighterDetails details)
        {
            InitializeComponent();
            FighterDetails = details;

            DataContext = this;


        }




        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //var gridPanel = FindVisualChild<UniformGrid>(FightersListView);
            //if (gridPanel != null)
            //{
            //    double width = e.NewSize.Width;
            //    if (width > 1000)
            //        gridPanel.Columns = 4;
            //    else if (width > 800)
            //        gridPanel.Columns = 3;
            //    else if (width > 600)
            //        gridPanel.Columns = 2;
            //    else
            //        gridPanel.Columns = 1;
            //}
        }

        // Helper function to find UniformGrid
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

    }
}
