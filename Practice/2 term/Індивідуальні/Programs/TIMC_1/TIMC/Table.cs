using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIMC
{
    public class Table
    {
        private List<double> _Intervals;
        private List<int> _Frequencies;

        public Table(List<double> intervals, List<int> frequencies)
        {
            _Intervals = intervals;
            _Frequencies = frequencies;
        }

        public List<double> Intervals
        {
            get { return _Intervals; }
        }
        public List<int> Frequencies
        {
            get { return _Frequencies; }
        }
    }
}
