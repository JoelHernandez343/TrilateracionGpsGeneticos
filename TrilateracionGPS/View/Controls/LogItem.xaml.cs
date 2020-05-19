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
using TrilateracionGPS.Model;

namespace TrilateracionGPS.View.Controls
{
    /// <summary>
    /// Interaction logic for LogItem.xaml
    /// </summary>
    public partial class LogItem : UserControl
    {
        public LogItem()
        {
            InitializeComponent();
        }

        public (int, double, double, double) Values
        {
            set
            {
                TitleTextBlock.Text = $"Ronda {value.Item1}";
                xField.Text = value.Item2.ToString();
                yField.Text = value.Item3.ToString();
                rField.Text = value.Item4.ToString();
            }
        }
    }
}
