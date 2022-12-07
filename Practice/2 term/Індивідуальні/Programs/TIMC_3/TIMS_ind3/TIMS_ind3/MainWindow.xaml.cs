using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using Microsoft.Win32;

namespace TIMS_ind3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region private fields

        private SortedDictionary<double, int> _inputData;
        private Dictionary<string, int> _histogramTable;
        private Dictionary<double, int> _frequencyTable;
        private int _n;
        private double _average;
        private double _variance;
        private double _standart;
        private double _dispersion;

        #endregion


        #region callbacks

        private void readButton_Click(object sender, RoutedEventArgs e)
        {
            _inputData = new SortedDictionary<double, int>();
            _histogramTable = new Dictionary<string, int>();
            _frequencyTable = new Dictionary<double, int>();
            _n = 0;

            OpenFileDialog dlg = new OpenFileDialog {DefaultExt = ".txt", Filter = "Text documents (.txt)|*.txt"};

            var result = dlg.ShowDialog();

            if (result == true)
            {
                var filename = dlg.FileName;

                using (var sr = File.OpenText(filename))
                {
                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() != "")
                        {
                            var tokens = line.Split(new char[] { ' ' });
                            try
                            {
                                _inputData.Add(double.Parse(tokens[0]), 1);
                            }
                            catch (ArgumentException)
                            {
                                _inputData[double.Parse(tokens[0])]++;
                            }
                            _n++;
                        }
                    }
                }
            }
            Assignment();
            Calculations();
            Hypothesis();
        }

        #endregion


        #region calculations

        private void Assignment()
        {

            numberTextBlock.Text = _n.ToString();
            inputDataGrid.DataContext = _inputData;

            var scope = (_inputData.Last().Key - _inputData.First().Key)/7;
            var from = _inputData.Keys.First();
            while (from < _inputData.Keys.Last())
            {
                var to = from + scope;
                var k =
                    _inputData.Where(element => element.Key >= @from && element.Key <= to).Sum(element => element.Value);
                _histogramTable.Add(Math.Round(from, 3) + "-" + Math.Round(to, 3), k);
                _frequencyTable.Add(Math.Round((from + to)/2, 3), k);
                from = to;
            }

            intervalsGrid.DataContext = _histogramTable;
            ((ColumnSeries) histogramChart.Series[0]).DataContext = _histogramTable;
        }

        private void Calculations()
        {
            _average = 0;
            foreach (var i in _frequencyTable)
            {
                _average += i.Key*i.Value;
            }
            _average /= _n;

            var dev = _inputData.Sum(i => Math.Pow(i.Key - _average, 2)*i.Value);
            _variance = dev/(_n - 1);

            _standart = Math.Sqrt(_variance);

            _dispersion = dev/_n;


            const double criticalExpectation = 2.68;
            var beginExpectation = _average - (_standart/Math.Sqrt(_n))*criticalExpectation;
            var endExpectation = _average + (_standart/Math.Sqrt(_n))*criticalExpectation;

            const double hiBegin = 1.6;
            const double hiEnd = 0.561;
            var beginDispersion = _variance/hiBegin;
            var endDispersion = _variance/hiEnd;

            outputTextBox.Text += String.Format("Вибіркове середнє: {0:0.000}", _average) + Environment.NewLine;
            outputTextBox.Text += String.Format("Вибіркова дисперсія: {0:0.000}", _dispersion) + Environment.NewLine;
            outputTextBox.Text += String.Format("Варіанса: {0:0.000}", _variance) + Environment.NewLine;
            outputTextBox.Text += String.Format("Стандарт: {0:0.000}", _standart) + Environment.NewLine;
            outputTextBox.Text += String.Format(
                "Довірчий інтервал для математичного сподівання: ({0:0.000} ; {1:0.000})"
                , beginExpectation, endExpectation) + Environment.NewLine;
            outputTextBox.Text += String.Format("Довірчий інтервал для дисперсії: ({0:0.000} ; {1:0.000})"
                                                , beginDispersion, endDispersion) + Environment.NewLine;
        }

        private void Hypothesis()
        {
            const double a = 3.0;
            const double criticalExpectation = 2.01;
            var empiricalExpectation = ((a - _average)/_standart)*Math.Sqrt(_n);

            outputTextBox.Text += "Гіпотеза про те, що середня температура на виході з реактора а = 3 " +
                                  "при рівні значущості 0.05" +"\nЕмпіричне: "+empiricalExpectation.ToString("F3")+"\nКритичне: "+criticalExpectation.ToString("F3")+ Environment.NewLine;
            outputTextBox.Text += empiricalExpectation < criticalExpectation
                                      ? "Приймається" + Environment.NewLine
                                      : "Не приймається" + Environment.NewLine;

            const double o = 0.5;
            const double hiBegin = 0.561;
            const double hiEnd = 1.6;
            var empiricalT = _variance /( o*o);
            outputTextBox.Text += "Твердження про рівність середньоквадратичного відхилення " +
                                  "випадкової величини Х , при рівні значущості 0.01, не перевищує σ0 = 0,5" + "\nЕмпіричне: " + empiricalT.ToString("F3") + "\nКритичні: " + hiBegin.ToString("F3") + " ; " + hiEnd.ToString("F3") +
                                  Environment.NewLine;
            outputTextBox.Text += (empiricalT > hiBegin && empiricalT < hiEnd)
                                      ? "Приймається" + Environment.NewLine
                                      : "Не приймається" + Environment.NewLine;
        }

        #endregion

    }
}
