using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrilateracionGPS.Model.Helpers
{
    class Help
    {
        // Simplification of a * a
        public static double Square(double a) => a * a;

        // Resolves ax^2 + bx + c = 0
        public static (double, double) QuadraticEquation(double a, double b, double c)
        {
            double discriminat = b * b - 4 * a * c;
            if (discriminat < 0)
                throw new Exception("El discriminante no es positivo.");

            double x1 = ((-b) + Math.Sqrt(discriminat)) / (2 * a);
            double x2 = ((-b) - Math.Sqrt(discriminat)) / (2 * a);

            return (x1, x2);
        }
    }
}
