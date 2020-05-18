using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrilateracionGPS.Model
{
    public class Circle
    {
        private double r;

        public double X { get; set; }
        public double Y { set; get; }
        public double R
        {
            set { r = value < 0 ? -value : value; }
            get => r;
        }

        public Circle(double x, double y, double r)
        {
            X = x;
            Y = y;
            R = r;
        }

        public Circle()
        {
            X = 0;
            Y = 0;
            R = 0;
        }
    }
}
