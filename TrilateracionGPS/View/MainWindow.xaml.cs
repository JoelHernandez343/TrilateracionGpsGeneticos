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
using TrilateracionGPS.Model;
using TrilateracionGPS.View.Controls;

namespace TrilateracionGPS.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddRestriction(0);
            AddRestriction(1);
            AddRestriction(2);
        }

        /// UI Methods
        private void AddRestrictionButton_Click(object sender, RoutedEventArgs e)
        {
            AddRestriction(RestrictionsStackPanel.Children.Count);
        }

        private void RemoveRestrictionButton_Click(object sender, RoutedEventArgs e)
        {
            var control = ((((sender as Button).Parent) as Grid).Parent as StackPanel).Parent as RestrictionControl;
            RemoveRestriction(control.Index);
        }

        private void TextChanged_Event(object sender, RoutedEventArgs e)
        {
            var t = sender as TextBox;

            if (t.Name == "TimerTextBox")
                ToggleConfigErrorStyle(t, CheckInteger, 100);
            else if (t.Name == "PrecisionTextBox")
                ToggleConfigErrorStyle(t, CheckInteger, 0);
            else if (t.Name == "PoblationSizeTextBox" || t.Name == "PoblationsTextBox")
                ToggleConfigErrorStyle(t, CheckInteger, 1);
            else
                ToggleConfigErrorStyle(t, CheckError);

        }

        private void ClearAllRestrictionButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (RestrictionControl c in RestrictionsStackPanel.Children)
            {
                c.Coordinate = new Coordinate();
            }
        }

        private void ClearConfigButton_Click(object sender, RoutedEventArgs e)
        {
            ClearConfiguration(TimerTextBox);
            ClearConfiguration(PrecisionTextBox);
            ClearConfiguration(PoblationSizeTextBox);
            ClearConfiguration(PoblationsTextBox);
            ClearConfiguration(ErrorTextBox);
            AbsErrRb.IsChecked = true;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAll())
                return; 
            


        }

        /// UI Operations

        // Validate
        public bool ValidateAll()
        {
            bool isValid = true;
            var badRestrictions = checkAllRestrictions();
            if (badRestrictions.Count != 0)
            {
                isValid = false;
                string m = "Existen errores en tus restricciones: ";

                for (int i = 0; i < badRestrictions.Count; ++i)
                    m += $"{badRestrictions[i] + 1}" + (i == badRestrictions.Count - 1 ? "" : ", ");
                m += ". Chécalas.";

                MessageBox.Show(m, "Errores en las restricciones.", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }

            if (!CheckAllGeneticConfig())
            {
                MessageBox.Show("Hay error(es) en la configuración", "Error en la configuración.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                isValid = false;
            }

            return isValid;
        }

        // Restrictions
        void AddRestriction(int index)
        {
            RestrictionsStackPanel.Children.Add(new RestrictionControl { Index = index, Coordinate = new Coordinate(), DeleteFunction = RemoveRestrictionButton_Click });
            UpdateIndexCanDelete();
        }
        void RemoveRestriction(int index)
        {
            RestrictionsStackPanel.Children.RemoveAt(index);
            UpdateIndexCanDelete();
        }
        void UpdateIndexCanDelete()
        {
            bool canDelete = RestrictionsStackPanel.Children.Count > 3;
            int i = 0;
            foreach (RestrictionControl c in RestrictionsStackPanel.Children)
            {
                c.CanDelete = canDelete;
                c.Index = i++;
            }

        }
        List<int> checkAllRestrictions()
        {
            var indexes = new List<int>();
            int i = 0;
            foreach(RestrictionControl c in RestrictionsStackPanel.Children)
            {
                if (!c.IsValid)
                    indexes.Add(i);
                i++;
            }

            return indexes;
        }

        // Genetics TextBox
        void ClearConfiguration(TextBox t)
        {
            RemoveErrorStyle(t);
            t.Text = "";
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

        // Evaluation of Genetics TextBox
        bool ToggleConfigErrorStyle(TextBox t, Func<string, int, string> checker, int begin)
        {
            string r = checker(t.Text, begin);

            if (r == "")
                RemoveErrorStyle(t);
            else
                SetErrorStyle(t, r);

            return r == "";
        }
        bool ToggleConfigErrorStyle(TextBox t, Func<string, string> errorChecker)
        {
            string r = errorChecker(t.Text);

            if (r == "")
                RemoveErrorStyle(t);
            else
                SetErrorStyle(t, r);

            return r == "";
        }

        string CheckInteger(string input, int begin)
        {
            int val;
            string m = $"Ingresa un entero mayor o igual a {begin}";
            try
            {
                val = int.Parse(input);
            }
            catch (Exception)
            {
                return m;
            }

            if (val < begin)
                return m;

            return "";
        }
        string CheckError(string input)
        {
            double val;
            string m = $"Ingresa un doble entre 0.0 y 1.0";
            try
            {
                val = double.Parse(input);
            }
            catch (Exception)
            {
                return m;
            }

            if (val < 0.0 || val > 1.0)
                return m;

            return "";
        }

        bool CheckAllGeneticConfig()
        {
            bool flag = true;

            flag &= ToggleConfigErrorStyle(TimerTextBox, CheckInteger, 100);
            flag &= ToggleConfigErrorStyle(PrecisionTextBox, CheckInteger, 0);
            flag &= ToggleConfigErrorStyle(PoblationSizeTextBox, CheckInteger, 1);
            flag &= ToggleConfigErrorStyle(PoblationsTextBox, CheckInteger, 1);
            flag &= ToggleConfigErrorStyle(ErrorTextBox, CheckError);

            return flag;
        }

    }
}
