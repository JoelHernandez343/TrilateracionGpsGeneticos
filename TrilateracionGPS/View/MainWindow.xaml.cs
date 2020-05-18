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
            addRestriction(0);
            addRestriction(1);
            addRestriction(2);
        }

        /// UI Methods
        private void AddRestrictionButton_Click(object sender, RoutedEventArgs e)
        {
            addRestriction(RestrictionsStackPanel.Children.Count);
        }

        private void RemoveRestrictionButton_Click(object sender, RoutedEventArgs e)
        {
            var control = ((((sender as Button).Parent) as StackPanel).Parent as StackPanel).Parent as RestrictionControl;
            removeRestriction(control.Index);
        }

        private void TextChanged_Event(object sender, RoutedEventArgs e)
        {
            var t = sender as TextBox;

            if (t.Name == "TimerTextBox")
                toggleConfigErrorStyle(t, checkInteger, 100);
            if (t.Name == "PrecisionTextBox")
                toggleConfigErrorStyle(t, checkInteger, 0);
            else if (t.Name == "PoblationSizeTextBox" || t.Name == "PoblationsTextBox")
                toggleConfigErrorStyle(t, checkInteger, 1);
            else
                toggleConfigErrorStyle(t, checkError);

        }

        private void ClearAllRestrictionButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(RestrictionControl c in RestrictionsStackPanel.Children)
            {
                c.Circle = new Circle();
            }
        }

        private void ClearConfigButton_Click(object sender, RoutedEventArgs e)
        {
            clearConfiguration(TimerTextBox);
            clearConfiguration(PrecisionTextBox);
            clearConfiguration(PoblationSizeTextBox);
            clearConfiguration(PoblationsTextBox);
            clearConfiguration(ErrorTextBox);
            AbsErrRb.IsChecked = true;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            checkAllGeneticConfig();
        }

        /// UI Operations
        
        // Restrictions
        void addRestriction(int index)
        {
            RestrictionsStackPanel.Children.Add(new RestrictionControl { Index = index, Circle = new Circle(), DeleteFunction = RemoveRestrictionButton_Click });
            updateIndexCanDelete();
        }
        void removeRestriction(int index)
        {
            RestrictionsStackPanel.Children.RemoveAt(index);
            updateIndexCanDelete();
        }   
        void updateIndexCanDelete()
        {
            bool canDelete = RestrictionsStackPanel.Children.Count > 3;
            int i = 0;
            foreach (RestrictionControl c in RestrictionsStackPanel.Children)
            {
                c.CanDelete = canDelete;
                c.Index = i++;
            }
            
        }

        // Genetics TextBox
        void clearConfiguration(TextBox t)
        {
            removeErrorStyle(t);
            t.Text = "";
        }
        void setErrorStyle(TextBox t, string m)
        {
            t.Style = (Style)FindResource("TextBoxError");
            t.ToolTip = new ToolTip { Content = m };
        }
        void removeErrorStyle(TextBox t)
        {
            t.Style = null;
            t.ToolTip = null;
        }

        // Evaluation of Genetics TextBox
        bool toggleConfigErrorStyle(TextBox t, Func<string, int, string> checker, int begin)
        {
            string r = checker(t.Text, begin);

            if (r == "")
                removeErrorStyle(t);
            else
                setErrorStyle(t, r);

            return r == "";
        }
        bool toggleConfigErrorStyle(TextBox t, Func<string, string> errorChecker)
        {
            string r = errorChecker(t.Text);

            if (r == "")
                removeErrorStyle(t);
            else
                setErrorStyle(t, r);

            return r == "";
        }

        string checkInteger(string input, int begin)
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
        string checkError(string input)
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

            if (val < 0.0 && val > 1.0)
                return m;

            return "";
        }

        bool checkAllGeneticConfig()
        {
            if (!toggleConfigErrorStyle(TimerTextBox, checkInteger, 100))
                return false;

            if (!toggleConfigErrorStyle(PrecisionTextBox, checkInteger, 0))
                return false;

            if (!toggleConfigErrorStyle(PoblationSizeTextBox, checkInteger, 1))
                return false;

            if (!toggleConfigErrorStyle(PoblationsTextBox, checkInteger, 1))
                return false;

            if (!toggleConfigErrorStyle(ErrorTextBox, checkError))
                return false;

            return true;
        }

    }
}
