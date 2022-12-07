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
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class TableDrawer : UserControl
    {
        public TableDrawer()
        {
            InitializeComponent();
        }

        private string GetInterval(double a, double b)
        {
            return "(" + a.ToString("0.0") + " ; " + b.ToString("0.0") + "]"; 
        }

        private void CreateTable(int n)
        {
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            for (int i = 0; i < n; ++i)
            {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = new GridLength(90);
                grid.ColumnDefinitions.Add(c);
            }
            for (int i = 0; i < 2; ++i)
            {
                RowDefinition c = new RowDefinition();
                c.Height = new GridLength(60);
                grid.RowDefinitions.Add(c);
            }
        }
        public void DrawTable(Table table)
        {
            int n = table.Frequencies.Count;

            CreateTable(n);

            for (int i = 0; i < n; ++i)
            {
                TextBlock tb1 = new TextBlock();
                tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tb1.Text = GetInterval(table.Intervals[i], table.Intervals[i + 1]);

                Grid.SetColumn(tb1, i);
                Grid.SetRow(tb1, 0);

                grid.Children.Add(tb1);


                TextBlock tb2 = new TextBlock();
                tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                tb2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tb2.Text = table.Frequencies[i].ToString();

                Grid.SetColumn(tb2, i);
                Grid.SetRow(tb2, 1);

                grid.Children.Add(tb2);
            }
        }
    }
}
