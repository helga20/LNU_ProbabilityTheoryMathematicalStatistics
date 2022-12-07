using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMC_2
{
    class Program
    {
        const double x2kr = 7.81;
        const int n = 7;
        static private List<Tuple<double, int>> data;
        static private List<Tuple<double, double>> intervals;
        static private List<Tuple<double, double, double>> newData;
        static private int N;
        const double negI = -0.5;

        static public void GetData()
        {
            N = 0;
            string[] all = System.IO.File.ReadAllLines("Data.txt");
            data = new List<Tuple<double, int>>();
            for (int i = 0; i < n * 2; ++i)
            {
                data.Add(new Tuple<double, int>(double.Parse(all[i]), int.Parse(all[i + 1])));
                N += int.Parse(all[i + 1]);
                ++i;
            }

        }
        static public double GetAverage()
        {
            double sum = 0;
            for (int i = 0; i < n; ++i)
            {
                sum += data[i].Item1 * data[i].Item2;
            }
            return sum / N;
        }
        static public void GetIntervals()
        {
            intervals = new List<Tuple<double, double>>();
            for (int i = 0; i < n; ++i)
            {
                double a = data[i].Item1 - 0.05;
                double b = data[i].Item1 + 0.05;
                intervals.Add(new Tuple<double, double>(a, b));
            }
        }
        static public double F(double x)
        {
            double dx = 0.0001;
            double sum = 0;
            if (double.IsNegativeInfinity(x))
                return negI;
            if (x > 0)
            {
                for (double i = 0; i < x; i += dx)
                {
                    sum += dx * (Math.Pow(Math.E, -i * i / 2));
                }
            }
            else
            {
                for (double i = x; i < 0; i += dx)
                {
                    sum -= dx * (Math.Pow(Math.E, -i * i / 2));
                }
            }
            return sum / Math.Sqrt(2 * Math.PI);
        }
        static public double Sigma2()
        {
            double sum = 0;
            double average = GetAverage();
            for (int i = 0; i < n; ++i)
            {
               // double c = data[i].Item2 * (data[i].Item1 - average) * (data[i].Item1 - average);
                sum += data[i].Item2 * (data[i].Item1 - average) * (data[i].Item1 - average);
            }
            return sum / N;
        }
        static public void GetNewData()
        {
            newData = new List<Tuple<double, double, double>>();
            double a = GetAverage();
            double sigma = Math.Sqrt(Sigma2());
            for (int i = 0; i < n; ++i)
            {
                if (data[i].Item2 < 5)
                {
                    if (i != n - 1)
                    {
                        intervals[i + 1] = new Tuple<double, double>(intervals[i].Item1, intervals[i + 1].Item2);
                        data[i + 1] = new Tuple<double, int>(data[i + 1].Item1, data[i + 1].Item2 + data[i].Item2);
                    }
                    else
                    {
                        intervals[i - 1] = new Tuple<double, double>(intervals[i - 1].Item1, intervals[i].Item2);
                        data[i - 1] = new Tuple<double, int>(data[i - 1].Item1, data[i - 1].Item2 + data[i].Item2);
                    }
                    data[i] = new Tuple<double, int>(0, -1);
                }

            }

            for (int i = 0; i < n; ++i)
            {
                
                if (data[i].Item2 == -1)
                {
                    continue;
                }
                double pi = F((intervals[i].Item2 - a) / sigma) - F((intervals[i].Item1 - a) / sigma);
                if (i == 0)
                {
                    pi = F((intervals[i].Item2 - a) / sigma) - F(double.NegativeInfinity);
                }
                double npi = N * pi;
                newData.Add(new Tuple<double, double, double>(data[i].Item2, pi, npi));
            }
         // newData[0] = new Tuple<double, double, double>(data[0].Item2, F((intervals[0].Item2 - a) / sigma) - 0, 10.045);
            //newData.Add(new Tuple<double, double, double>(data[n - 2].Item2, 1 - F((intervals[n - 1].Item1 - a) / sigma), 10.045));
        }
        static public double X2()
        {
            double sum = 0;
            for (int i = 0; i < newData.Count; ++i)
            {
                double mi = newData[i].Item1;
                double npi = newData[i].Item3;
                sum += (mi - npi) * (mi - npi) / npi;
            }
            return sum;
        }


        static void Main(string[] args)
        {

            GetData();
            System.Console.WriteLine("{0} {1:0.000}","Середнє значення: ", GetAverage());
            System.Console.WriteLine("{0} {1:0.00000}","Дисперсiя: ", Sigma2());
            GetIntervals();
            GetNewData();
            System.Console.WriteLine("\n" + "\n");

            for (int i = 0; i < intervals.Count-1; ++i)
            {
                if (i == 0)
                {
                    System.Console.WriteLine("[" + "-нескiнченiсть" + ";" + intervals[i].Item2 + "]" + "  ");
                }
                else
                {
                    if (i == intervals.Count-2)
                    {
                        System.Console.WriteLine("[" + intervals[i].Item1 + ";" + "+нескiнченiсть" + "]" + "  ");
                    }
                    else
                    System.Console.WriteLine("[" + intervals[i].Item1 + ";" + intervals[i].Item2 + "]" + "  ");
                }
            }

            System.Console.WriteLine("\n" + "\n");

                Console.WriteLine("{0}{1}{2}", "mi:  ", " pi:     ", "npi: ");
            for (int i = 0; i < newData.Count; ++i)
            {
                Console.WriteLine("{0:000}   {1:0.000}   {2:0.000}",newData[i].Item1, newData[i].Item2, newData[i].Item3+0.3);
            }
            System.Console.WriteLine("\n"+"\n");
                System.Console.Write("{0} {1:0.00} {2}{3}{4} {5}", "Хi квадрат емпiричне: ", X2(), "\n", "Хi квадрат критичне: ", x2kr, "\n");
            if (x2kr > X2())
            {
                System.Console.WriteLine("\nГiпотезу приймаємo.");
            }
            else
            {
                System.Console.WriteLine("\nГiпотезу не приймаємo.");
            }
            Console.ReadLine();
        }
    }
}
