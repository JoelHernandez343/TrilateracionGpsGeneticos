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

namespace TrilateracionGPS.View.Controls
{
    /// <summary>
    /// Interaction logic for LogItemDebbug.xaml
    /// </summary>
    public partial class LogItemDebbug : UserControl
    {
        public LogItemDebbug()
        {
            InitializeComponent();
        }

        public string Message
        {
            set
            {
                MainTextBlock.Text = value;
            }
        }

    }
}
