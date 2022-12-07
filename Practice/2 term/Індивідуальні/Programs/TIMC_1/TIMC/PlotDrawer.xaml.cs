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
using System.Collections.ObjectModel;

namespace TIMC
{
    /// <summary>
    /// Interaction logic for PlotDrawer.xaml
    /// </summary>
    public partial class PlotDrawer : UserControl
    {
        public PlotDrawer()
        {
            InitializeComponent();
            plot = new ObservableCollection<UIElement>();
            Width = 800;
            Height = 500;
            grid.DataContext = this;

            Stroke = Brushes.Black;
            StrokeThickness=1;
        }

        private List<Tuple<double, int>> GetFrequencies(List<double> data)
        {
            data.Sort();

            List<Tuple<double, int>> result = new List<Tuple<double, int>>();
            int counter = 1;
            for (int i = 1; i < data.Count; ++i)
            {
                if (data[i] == data[i - 1])
                {
                    counter++;
                }
                else 
                {
                    result.Add(new Tuple<double, int>(data[i-1], counter));
                    counter = 1;
                }
            }
            result.Add(new Tuple<double, int>(data[data.Count-1], counter));
            return result;
        }

        
        public ObservableCollection<UIElement> plot
        {
            get { return (ObservableCollection<UIElement>)GetValue(plotProperty); }
            set { SetValue(plotProperty, value); }
        }

        // Using a DependencyProperty as the backing store for plot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty plotProperty =
            DependencyProperty.Register("plot", typeof(ObservableCollection<UIElement>), typeof(PlotDrawer), new UIPropertyMetadata(new ObservableCollection<UIElement>()));

        Brush Stroke { get; set; }
        double StrokeThickness { get; set; }

        

        double dx, dy;
        double sx, sy;
        double Sx, Sy;

        double minx, maxx, miny, maxy;

        private Point Transform(double x, double y)
        {
            return new Point(x * sx * Sx + dx, y * sy * Sy + dy);
        }

        private Line GetDashedLine(double x1, double y1, double x2, double y2)
        {
            Point A = Transform(x1,y1);
            Point B = Transform(x2,y2);

            Line line = new Line()
            {
                X1 = A.X,
                Y1 = A.Y,
                X2 = B.X,
                Y2 = B.Y,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness/2,
                StrokeDashArray = { 4, 4 }
            };
            return line;
        }

        private Line GetWideLine(double x1, double y1, double x2, double y2)
        {
            Point A = Transform(x1, y1);
            Point B = Transform(x2, y2);

            Line line = new Line()
            {
                X1 = A.X,
                Y1 = A.Y,
                X2 = B.X,
                Y2 = B.Y,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness * 3
            };
            return line;
        }

        private Line GetColoredLine(double x1, double y1, double x2, double y2)
        {
            Point A = Transform(x1, y1);
            Point B = Transform(x2, y2);

            Line line = new Line()
            {
                X1 = A.X,
                Y1 = A.Y,
                X2 = B.X,
                Y2 = B.Y,
                Stroke = Brushes.Green,
                StrokeThickness = StrokeThickness * 2
            };
            return line;
        }

        private Line GetColoredDashedLine(double x1, double y1, double x2, double y2)
        {
            Point A = Transform(x1, y1);
            Point B = Transform(x2, y2);

            Line line = new Line()
            {
                X1 = A.X,
                Y1 = A.Y,
                X2 = B.X,
                Y2 = B.Y,
                Stroke = Brushes.Green,
                StrokeDashArray = { 4, 4 },
                StrokeThickness = StrokeThickness * 2
            };
            return line;
        }

        private Line GetLine(double x1, double y1, double x2, double y2)
        {
            Point A = Transform(x1,y1);
            Point B = Transform(x2,y2);

            Line line = new Line()
            {
                X1 = A.X,
                Y1 = A.Y,
                X2 = B.X,
                Y2 = B.Y,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };
            return line;
        }

        private Rectangle GetRectangle(double x, double y)
        {
            Point a = Transform(x, y);
            Point b = Transform(x, 0);
            Rectangle rectangle = new Rectangle()
            {
                Margin = new Thickness(a.X - 2, a.Y, 0, 0),
                Width = 4,
                Height = b.Y - a.Y,
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                Fill = Brushes.LightBlue
            };
            return rectangle;
        }

        private bool IsInside(double x, double y)
        {
            Point a = Transform(x, y);
            if (a.X < 0 || a.Y < 0 || a.X >= Width || a.Y >= Height) return false;
            return true;
        }

        private void DrawAxes()
        {
            plot.Add(GetWideLine(0, miny, 0, maxy));
            plot.Add(GetWideLine(minx, 0, maxx, 0));

            double d = (maxx - minx) / 10;

            for (int i = -10; i <= 10; i++)
            {
                if (IsInside(d * i, 0))
                {
                    plot.Add(GetDashedLine(d * i, miny, d * i, maxy));
                    Label t = new Label();
                    t.Content = (i * d).ToString("0.0");

                    Point p = Transform(d*i,0);

                    t.Margin = new Thickness(p.X-1, p.Y-1, 0, 0);
                    plot.Add(t);
                }

            }

            d = (maxy - miny) / 10;

            for (int i = -10; i <= 10; i++)
            {
                if (IsInside(0, d * i))
                {
                    plot.Add(GetDashedLine(minx, d * i, maxx, d * i));

                    Label t = new Label();
                    t.Content = (i * d).ToString("0.0");

                    Point p = Transform(0, d * i);

                    t.Margin = new Thickness(p.X - 1, p.Y - 1, 0, 0);
                    plot.Add(t);
                }
            }
        }
        
        //private List<Tuple<double, int>> Prepare(List<double> data)
        //{
        //    plot.Clear();

        //    List<Tuple<double, int>> frequencies = GetFrequencies(data);


        //    maxy = 0;
        //    miny = 0;
        //    for (int i = 0; i < frequencies.Count; i++)
        //    {
        //        maxy = Math.Max(maxy, frequencies[i].Item2);
        //    }

        //    maxx = Math.Max(data.Max(), 0);
        //    minx = Math.Min(data.Min(), 0);

        //    minx--;
        //    maxx++;
        //    miny--;
        //    maxy++;

        //    while ((maxy - miny) % 10 != 0) maxy++;
        //    int min = (int)minx;
        //    int max = (int)maxx;
        //    while ((max - min) % 10 != 0)
        //    {
        //        max++;
        //        if ((max - min) % 10 != 0) min--;
        //    }
        //    maxx = max;
        //    minx = min;

        //    sx = 1;
        //    sy = -1;


        //    dx = -minx * Width / (maxx - minx);
        //    dy = maxy * Height / (maxy - miny);

        //    Sx = Math.Min((Width - dx) / (sx * maxx), -dx / (sx * minx));
        //    Sy = Math.Min((Height - dy) / (sy * miny), -dy / (sy * maxy));
            
        //    DrawAxes();
        //    return frequencies;
        //}
        private void Prepare(Table data)
        {
            plot.Clear();

            maxy = 0;
            miny = 0;
            for (int i = 0; i < data.Frequencies.Count; i++)
            {
                maxy = Math.Max(maxy, data.Frequencies[i]);
            }

            maxx = 0;
            minx = 0;

            for (int i = 0; i < data.Intervals.Count; i++)
            {
                maxx = Math.Max(maxx, data.Intervals[i]);
                minx = Math.Min(minx, data.Intervals[i]);
            }

            minx--;
            maxx++;
            miny--;
            maxy++;

            while ((maxy - miny) % 10 != 0) maxy++;
            int min = (int)minx;
            int max = (int)maxx;
            while ((max - min) % 10 != 0)
            {
                max++;
                if ((max - min) % 10 != 0) min--;
            }
            maxx = max;
            minx = min;

            sx = 1;
            sy = -1;


            dx = -minx * Width / (maxx - minx);
            dy = maxy * Height / (maxy - miny);

            Sx = Math.Min((Width - dx) / (sx * maxx), -dx / (sx * minx));
            Sy = Math.Min((Height - dy) / (sy * miny), -dy / (sy * maxy));

            DrawAxes();
        }

        public void DrawFrequenciesPlot(Table data)
        {

            //List<Tuple<double, int>> frequencies = Prepare(data);

            //for (int i = 0; i < frequencies.Count; i++)
            //{                
            //    plot.Add(GetColoredLine(frequencies[i].Item1, frequencies[i].Item2, frequencies[i].Item1, 0));
            //}
            Prepare(data);
            for (int i = 0; i < data.Frequencies.Count; i++)
            {
                plot.Add(GetColoredLine((data.Intervals[i] + data.Intervals[i + 1]) / 2, 0,(data.Intervals[i] + data.Intervals[i + 1]) / 2,data.Frequencies[i]));
            }

        }
        public void DrawFrequenciesPolygon(Table data)
        {
            //List<Tuple<double, int>> frequencies = Prepare(data);

            //for (int i = 1; i < frequencies.Count; i++)
            //{
            //    plot.Add(GetColoredLine(frequencies[i - 1].Item1, frequencies[i - 1].Item2, frequencies[i].Item1, frequencies[i].Item2));
            //}
            Prepare(data);
            for (int i = 0; i < data.Frequencies.Count; i++)
            {
                plot.Add(GetColoredDashedLine((data.Intervals[i] + data.Intervals[i + 1]) / 2, 0, (data.Intervals[i] + data.Intervals[i + 1]) / 2, data.Frequencies[i]));
            }
            for (int i = 0; i < data.Frequencies.Count-1; i++)
            {
                plot.Add(GetColoredLine((data.Intervals[i] + data.Intervals[i + 1]) / 2, data.Frequencies[i], (data.Intervals[i+1] + data.Intervals[i + 2]) / 2, data.Frequencies[i+1]));
            }
           
        }
        public void DrawHistogram(Table data)
        {
            Prepare(data);
            for (int i = 0; i < data.Frequencies.Count; i++)
            {
                plot.Add(GetColoredLine(data.Intervals[i], data.Frequencies[i], data.Intervals[i + 1], data.Frequencies[i]));
                if (i != data.Frequencies.Count - 1)
                {
                    plot.Add(GetColoredDashedLine(data.Intervals[i + 1], data.Frequencies[i], data.Intervals[i + 1], data.Frequencies[i + 1]));
                }
            }
        }
    }
}
