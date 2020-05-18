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
    /// Interaction logic for TooltipContentControl.xaml
    /// </summary>
    public partial class TooltipContentControl : UserControl
    {
        public TooltipContentControl()
        {
            InitializeComponent();
        }

        private string errorMessage;
        public string ErrorMessage
        {
            set
            {
                errorMessage = value;
                if (errorMessageTextBlock != null)
                    errorMessageTextBlock.Text = errorMessage;
            }
        }

        public TooltipContentControl(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
