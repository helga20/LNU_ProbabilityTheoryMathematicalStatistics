using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIMC
{
    class Calculator
    {
        private List<double> _Data;

        public Calculator(List<double> data)
        {
            _Data = data;
            _Data.Sort();

        }

        public double Mediana()
        {
            double result;
            if (_Data.Count() % 2 != 0)
            {
                result = _Data[_Data.Count() / 2];
            }
            else
            {
                result = (_Data[(_Data.Count() + 1) / 2] + _Data[(_Data.Count() - 1) / 2]) / 2;
            }
            return result;
        }

        //public double Moda()
        //{
        //    int count = 1;
        //    int max = 0;
        //    double res = 0;
        //    for (int i = 1; i < _Data.Count(); ++i)
        //    {
        //        if (_Data[i - 1] == _Data[i])
        //        {
        //            count++;
        //        }
        //        else
        //        {
        //            if (count > max)
        //            {
        //                max = count;
        //                res = _Data[i - 1];
        //            }
        //            count = 1;
        //        }
        //    }
        //    if (count > max)
        //    {
        //        res = _Data[_Data.Count() - 1];
        //    }
        //    return res;
        //}
        public List<double> Moda()
        {
            int count = 1;
            int max = 0;
            List<double> res = new List<double>();
            for (int i = 1; i < _Data.Count(); ++i)
            {
                if (_Data[i - 1] == _Data[i])
                {
                    count++;
                }
                else
                {
                    if (count > max)
                    {
                        max = count;
                    }
                    count = 1;
                }
            }
            if (count == max)
            {
                res.Add(_Data[_Data.Count() - 1]);
            }
            count = 0;
            for (int i = 1; i < _Data.Count(); ++i)
            {
                if (_Data[i - 1] == _Data[i])
                {
                    count++;
                }
                else
                {
                    if (count == max)
                    {
                        res.Add(_Data[i - 1]);
                    }
                    count = 1;
                }
            }

            return res;

        }

        public double Average()
        {
            double result = 0;
            foreach (double i in _Data)
            {
                result += i;
            }

            return result / _Data.Count();
        }

        public double Variance()
        {
            double result = 0;
            double average = this.Average();
            foreach (double i in _Data)
            {
                result += (i - average) * (i - average);
            }
            double a = result / (_Data.Count() - 1); 
            return a;
        }

        public double Standart()
        {
            return Math.Sqrt(this.Variance());
        }

        public double Variation()
        {
            return this.Standart() / this.Average();
        }

        public double Scope()
        {
            return (_Data.Max() - _Data.Min());
        }

        private List<double> Quantiles(int n)
        {
            if (_Data.Count() % n != 0)
            {
                return null;
            }
            List<double> quantiles = new List<double>();
            for (int i = 1; i < n; ++i)
                quantiles.Add(_Data[_Data.Count() * i / n-1]);
            return quantiles;

        }

        public List<double> Quartiles()
        {
            return this.Quantiles(4);
        }
        public List<double> Octiles()
        {
            return this.Quantiles(8);
        }
        public List<double> Deciles()
        {
            return this.Quantiles(10);
        }
        public List<double> Centiles()
        {
            return this.Quantiles(100);
        }
        public List<double> Mililes()
        {
            return this.Quantiles(1000);
        }

        private double Moment(int k)
        {
            double result = 0;
            double average = this.Average();
            foreach (double i in _Data)
            {
                result += Math.Pow(i - average, k);
            }
            return result / _Data.Count();

        }
        public double Moment2()
        {
            return this.Moment(2);
        }
        public double Moment3()
        {
            return this.Moment(3);
        }
        public double Moment4()
        {
            return this.Moment(4);
        }

        public double Asymmetry()
        {
            return (this.Moment3() / Math.Pow(this.Moment2(), 1.5));
        }
        public double Excess()
        {
            return (this.Moment4() / Math.Pow(this.Moment2(), 2) - 3);
        }


    }
}
