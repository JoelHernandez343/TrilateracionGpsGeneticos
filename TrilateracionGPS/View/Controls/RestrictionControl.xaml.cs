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
using TrilateracionGPS.Model.Data;

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
        private int index;
        public int Index
        {
            get => index;
            set
            {
                index = value;
                TitleTextBlock.Text = $"Restriccion {Index + 1}:";
            }
        }

        public Coordinate Coordinate
        {
            get => new Coordinate
            {
                Latitude = double.Parse(latitudeField.Text),
                Longitude = double.Parse(longitudeField.Text),
                Distance = double.Parse(distanceField.Text)
            };
            set
            {
                latitudeField.Text = value.Latitude.ToString();
                longitudeField.Text = value.Longitude.ToString();
                distanceField.Text = value.Distance.ToString();
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
            IsValid = CheckAllTextBox();
        }

        private void TextKeyFocus_Event(object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        bool CheckAllTextBox()
        {
            bool flag = true;
            flag &= ToggleTextBoxStyle(latitudeField);
            flag &= ToggleTextBoxStyle(longitudeField);
            flag &= ToggleTextBoxStyle(distanceField);

            return flag;
        }
        bool ToggleTextBoxStyle(TextBox t)
        {
            string m;
            if (t.Name == "distanceField")
                m = CheckDistance(t.Text);
            else
                m = CheckValidDouble(t.Text);

            if (m == "")
                RemoveErrorStyle(t);
            else
                SetErrorStyle(t, m);

            return m == "";
        }

        void SetErrorStyle(TextBox t, string m)
        {
            t.Style = (Style)FindResource("TextBoxError");

            var g = new StackPanel();
            g.Children.Add(new TooltipContentControl { ErrorMessage = m });

            t.ToolTip = new ToolTip { Content = g };
        }
        void RemoveErrorStyle(TextBox t)
        {
            t.Style = null;
            t.ToolTip = null;
        }
        
        string CheckValidDouble(string input)
        {
            double val;
            string m = $"Ingresa un double válido.";
            try
            {
                val = double.Parse(input);
            }
            catch (Exception)
            {
                return m;
            }

            return "";
        }
        string CheckDistance(string input)
        {
            double val;
            string m = $"Ingresa un doble mayor o igual a 0.0.";
            try
            {
                val = double.Parse(input);
            }
            catch (Exception)
            {
                return m;
            }

            if (val < 0.0)
                return m;

            return "";
        }
    }
}
