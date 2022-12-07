using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pr1
{
    public class Kubik
    {
        string name;
        int colors;
        double volume;
        bool normality;

        public Kubik(string name, int colors, double volume, bool normality)
        {
            this.name = name;
            this.colors = colors;
            this.volume = volume;
            this.normality = normality;
        }
        public Kubik()
            : this("", 0, 0, false)
        {
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
        public int Colors
        {
            get
            {
                return colors;
            }
        }
        public double Volume
        {
            get
            {
                return volume;
            }
        }
        public bool Normality
        {
            get
            {
                return normality;
            }
        }


    }
}
