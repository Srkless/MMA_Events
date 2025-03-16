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
using System.Windows.Shapes;

namespace MMA_Events.View
{
    /// <summary>
    /// Interaction logic for BaseView.xaml
    /// </summary>
    public abstract partial class BaseWindow : Window
    {
        public abstract Frame MainFrame { get; }
        public abstract Button bButton { get; }
        public BaseWindow()
        {
           
        }


    }
}
