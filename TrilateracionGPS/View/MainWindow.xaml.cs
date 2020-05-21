using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TrilateracionGPS.Model;
using TrilateracionGPS.Model.Converters;
using TrilateracionGPS.Model.Data;
using TrilateracionGPS.Model.Genetic;
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
            PrevMessageText.Visibility = Visibility.Visible;
            ResultGrid.Visibility = Visibility.Collapsed;
            ErrorMessageGrid.Visibility = Visibility.Collapsed;
        }

        CancellationTokenSource timer;

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
                ToggleConfigErrorStyle(t, CheckInteger, 100, int.MaxValue);
            else if (t.Name == "PrecisionTextBox")
                ToggleConfigErrorStyle(t, CheckInteger, 0, int.MaxValue);
            else if (t.Name == "PoblationSizeTextBox")
                ToggleConfigErrorStyle(t, CheckInteger, 1, int.MaxValue);
            else if (t.Name == "PoblationsTextBox")
                ToggleConfigErrorStyle(t, CheckInteger, 1, 100);
            else
                ToggleConfigErrorStyle(t, CheckError);

        }

        private void TextKeyFocus_Event(object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox).SelectAll();
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

        private void SetProgressBar(int timeout)
        {
            TimeProgressBar.Value = 0;
            Duration dr = new Duration(TimeSpan.FromMilliseconds(timeout));
            DoubleAnimation da = new DoubleAnimation(100, dr);
            TimeProgressBar.IsIndeterminate = false;
            TimeProgressBar.BeginAnimation(ProgressBar.ValueProperty, da);

        }

        private void StopProgressBar()
        {
            TimeProgressBar.BeginAnimation(ProgressBar.ValueProperty, null);
            TimeProgressBar.Value = 100;
        }

        private void ToggleCalculateButtonFunction()
        {
            if ((string)CalculateButton.Content == "CALCULAR")
            {
                CalculateButton.Click -= CalculateButton_Click;
                CalculateButton.Click += CancelButton_Click;
                CalculateButton.Content = "CANCELAR";
                CalculateButton.Foreground = Brushes.Red;
            }
            else
            {
                CalculateButton.Click -= CancelButton_Click;
                CalculateButton.Click += CalculateButton_Click;
                CalculateButton.Content = "CALCULAR";
                CalculateButton.ClearValue(ForegroundProperty);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            timer?.Cancel();
        }
        
        private async void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAll())
                return;

            PrevMessageText.Visibility = Visibility.Collapsed;
            ResultGrid.Visibility = Visibility.Visible;

            ElapsedTimeTextBlock.Text = "--.--";
            LatitudeTextBlock.Text = "--.--";
            LongitudeTextBlock.Text = "--.--";

            ToggleCalculateButtonFunction();

            var circles = new List<Circle>();

            foreach (RestrictionControl r in RestrictionsStackPanel.Children)
            {
                circles.Add(CoordinateCircleConverter.CoordinateToCircle(r.Coordinate));
            }

            int n = int.Parse(PrecisionTextBox.Text);
            int rounds = int.Parse(PoblationsTextBox.Text);
            int size = int.Parse(PoblationSizeTextBox.Text);
            double error = double.Parse(ErrorTextBox.Text);
            bool abs = (bool)AbsErrRb.IsChecked;

            LogStackPanel.Children.Clear();

            int timeout = int.Parse(TimerTextBox.Text);
            timer = new CancellationTokenSource();
            timer.CancelAfter(timeout);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            SetProgressBar(timeout);

            try
            {
                var res = await Task.Run(() => Genetics.Calculate(circles.ToArray(), n, rounds, size, error, !abs, UpdateLogCrossThread, UpdateLogCrossThread, timer.Token), timer.Token);

                ResultsStackPanel.Visibility = Visibility.Visible;
                ErrorMessageGrid.Visibility = Visibility.Collapsed;

                var result = CoordinateCircleConverter.CircleToCoordinate(new Circle
                {
                    X = res.Item2,
                    Y = res.Item3
                });

                LatitudeTextBlock.Text = result.Latitude.ToString();
                LongitudeTextBlock.Text = result.Longitude.ToString();

            }
            catch (Exception ex)
            {
                ResultsStackPanel.Visibility = Visibility.Collapsed;
                ErrorMessageGrid.Visibility = Visibility.Visible;

                if (ex is TimeoutException)
                    ErrorMessageTextBlock.Text = "Se acabo el tiempo.";
                else
                    ErrorMessageTextBlock.Text = $"Ocurrió una excepción de tipo {ex.GetType().Name}\nMensaje: {ex.Message}";
            }

            StopProgressBar();
            sw.Stop();
            ElapsedTimeTextBlock.Text = sw.Elapsed.ToString();

            ToggleCalculateButtonFunction();

            timer = null;
        }
        
        // Cross thread operations
        private delegate void UpdateLogCallback((int, double, double, double) m);
        public void UpdateLogCrossThread((int, double, double, double) m)
        {
            Dispatcher.BeginInvoke(new UpdateLogCallback(AddLogToStackPanel), System.Windows.Threading.DispatcherPriority.Render, new object[] { m });
        }
        private void AddLogToStackPanel((int, double, double, double) m)
        {
            LogStackPanel.Children.Add(new LogItem { Values = m });
        }

        private delegate void UpdateLogItemDebbugCallback(string m);
        public void UpdateLogCrossThread(string m)
        {
            Dispatcher.BeginInvoke(new UpdateLogItemDebbugCallback(AddLogToStackPanel), System.Windows.Threading.DispatcherPriority.Render, new object[] { m });
        }
        private void AddLogToStackPanel(string m)
        {
            LogStackPanel.Children.Add(new LogItemDebbug { Message = m });
        }

        /// UI Operations

        // Validate
        public bool ValidateAll()
        {
            bool isValid = true;
            var badRestrictions = CheckAllRestrictions();
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
        List<int> CheckAllRestrictions()
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
        bool ToggleConfigErrorStyle(TextBox t, Func<string, int, int, string> checker, int begin, int limit)
        {
            string r = checker(t.Text, begin, limit);

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

        string CheckInteger(string input, int begin, int limit)
        {
            int val;
            string m = limit == int.MaxValue ? $"Ingresa un entero mayor o igual a {begin}" : $"Ingresa un entero entre {begin} y {limit}";
            try
            {
                val = int.Parse(input);
            }
            catch (Exception)
            {
                return m;
            }

            if (val < begin || val > limit)
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

            flag &= ToggleConfigErrorStyle(TimerTextBox, CheckInteger, 100, int.MaxValue);
            flag &= ToggleConfigErrorStyle(PrecisionTextBox, CheckInteger, 0, int.MaxValue);
            flag &= ToggleConfigErrorStyle(PoblationSizeTextBox, CheckInteger, 1, int.MaxValue);
            flag &= ToggleConfigErrorStyle(PoblationsTextBox, CheckInteger, 1, 100);
            flag &= ToggleConfigErrorStyle(ErrorTextBox, CheckError);

            return flag;
        }

    }
}
