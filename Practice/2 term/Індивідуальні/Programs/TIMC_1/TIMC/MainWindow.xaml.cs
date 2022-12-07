using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TIMC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(MainWindow), new PropertyMetadata("Data.txt"));



        private List<double> GetData()
        {

            List<double> data = new List<double>();
            string[] s = System.IO.File.ReadAllLines(FilePath);
            for (int i = 0; i < s.Length; ++i)
            {
                data.Add(double.Parse(s[i]));
            }
            return data;

        }
        private Table CreateTable(List<double> data)
        {
            double min = data.Min();
            double max = data.Max();
            double length = (max - min) / 9;
            data.Sort();
            List<double> intervals = new List<double>();
            List<int> frequencies = new List<int>();

            for (int i = 0; i <= 9; ++i)
            {
                intervals.Add(min + i * length);
            }
            for (int i = 0; i < 9; ++i)
            {
                frequencies.Add(0);
            }
            int index = 0;
            for (int i = 0; i < data.Count; ++i)
            {
                if (data[i] <= intervals[index + 1])
                {
                    frequencies[index]++;
                }
                else
                {
                    index++;
                    i--;
                }
            }
            Table table = new Table(intervals, frequencies);
            return table;
        }
        public MainWindow()
        {

            InitializeComponent();

            grid.DataContext = this;


            createTable_Click_1(null, null);
            calculate_Click_1(null, null);

        }

        

        private void createTable_Click_1(object sender, RoutedEventArgs e)
        {
            tableDrawer.DrawTable(CreateTable(GetData()));
        }

        private void drawPlot_Click_1(object sender, RoutedEventArgs e)
        {
            plotDrawer.DrawFrequenciesPlot(CreateTable(GetData()));
        }

        private void drawPolygon_Click_1(object sender, RoutedEventArgs e)
        {
            plotDrawer.DrawFrequenciesPolygon(CreateTable(GetData()));
        }

        private void drawHistogram_Click_1(object sender, RoutedEventArgs e)
        {
            plotDrawer.DrawHistogram(CreateTable(GetData()));
        }

        private void calculate_Click_1(object sender, RoutedEventArgs e)
        {
            Calculator c = new Calculator(GetData());
            
            median.Content = c.Mediana().ToString();
            average.Content = c.Average().ToString("0.000");
            variance.Content = c.Variance().ToString("0.000");
            standart.Content = c.Standart().ToString("0.000");
            variation.Content = c.Variation().ToString("0.000");
            scope.Content = c.Scope().ToString();
            moment2.Content = c.Moment2().ToString("0.000");
            moment3.Content = c.Moment3().ToString("0.000");
            moment4.Content = c.Moment4().ToString("0.000");
            asymmetry.Content = c.Asymmetry().ToString("0.000");
            excess.Content = c.Excess().ToString("0.000");

            List<double> list = new List<double>();
            string s = " ";

            list = c.Moda();
            foreach (double d in list)
            {
                s = s + d.ToString() + ";  ";
            }
            moda.Content = s;

            list = c.Quartiles();
            if (list != null)
            {
                s = " ";
                foreach (double d in list)
                {
                    s = s + d.ToString() + ";  ";
                }
                quartiles.Content = s;
            }
            else quartiles.Content = "нема";


            list = c.Octiles();
            if (list != null)
            {
                s = " ";
                foreach (double d in list)
                {
                    s = s + d.ToString() + ";  ";
                }
                octiles.Content = s;
            }
            else octiles.Content = "нема";


            list = c.Deciles();
            if (list != null)
            {
                s = " ";
                foreach (double d in list)
                {
                    s = s + d.ToString() + ";  ";
                }
                deciles.Content = s;

                System.IO.File.WriteAllText(@"WriteText.txt", s);

            }
            else deciles.Content = "нема";


            list = c.Centiles();
            if (list != null)
            {
                s = " ";
                for (int j = 1; j <= 10; ++j)
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        if (((j * 10) + i - 10) != 99)
                            s = s + list[(j * 10) + i - 10].ToString() + ";  ";
                    }
                    if (j != 10)
                    {
                        s += "\n";
                    }
                }                
                centiles.Content = s;
            }
            else centiles.Content = "нема";

            list = c.Mililes();
            if (list != null)
            {
                s = " ";
                foreach (double d in list)
                {
                    s = s + d.ToString() + ";  ";
                }
                mililes.Content = s;
            }
            else mililes.Content = "нема";




        }
    }
}
