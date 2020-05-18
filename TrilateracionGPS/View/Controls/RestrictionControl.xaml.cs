using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for RestrictionControl.xaml
    /// </summary>
    public partial class RestrictionControl : UserControl
    {
        public RestrictionControl()
        {
            InitializeComponent();
        }

        // Properties
        private int index { get; set; }
        public int Index
        {
            get => index;
            set
            {
                index = value;
                TitleTextBlock.Text = $"Restriccion {Index + 1}:";
            }
        }

        public Circle Circle
        {
            get => new Circle { 
                X = double.Parse(xField.Text),
                Y = double.Parse(yField.Text),
                R = double.Parse(rField.Text)
            };

            set
            {
                xField.Text = value.X.ToString();
                yField.Text = value.Y.ToString();
                rField.Text = value.R.ToString();
            }
        }

        public bool IsValid { get; set; }

        public RoutedEventHandler DeleteFunction
        {
            set => DeleteRestrictionButton.Click += value;
        }

        private bool canDelete;
        public bool CanDelete
        {
            get => canDelete;
            set
            {
                canDelete = value;
                DeleteRestrictionButton.IsEnabled = canDelete;
            }
        }

        private void TextChanged_Event(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            bool val = true;

            try
            {
                double.Parse(textBox.Text);
            } catch (Exception)
            {
                val = false;
            }

            IsValid = val;

            if (!IsValid)
                textBox.Style = (Style)FindResource("TextBoxError");
            else
                textBox.Style = null;
        }
    }
}
