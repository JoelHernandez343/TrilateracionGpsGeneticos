using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrilateracionGPS.Model.Helpers;

namespace TrilateracionGPS.Model.Data
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y) { X = x; Y = y; }

        public static double DistanceBetweenTwoPoints(double px, double py, double qx, double qy)
            => Math.Sqrt(Help.Square(px - qx) + Help.Square(py - qy));

        public static double DistanceBetweenTwoPoints(Point p, Point q)
            => Math.Sqrt(Help.Square(p.X - q.X) + Help.Square(p.Y - q.Y));
    }
}
