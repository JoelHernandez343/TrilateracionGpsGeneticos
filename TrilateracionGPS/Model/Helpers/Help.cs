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
            double discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
                throw new Exception("El discriminante no es positivo.");

            double x1 = ((-b) + Math.Sqrt(discriminant)) / (2 * a);
            double x2 = ((-b) - Math.Sqrt(discriminant)) / (2 * a);

            return (x1, x2);
        }

        // Module operation
        public static double Mod(double a, double n) => a < 0 ? (n - ((-a)) % n) % n : a % n;

        // Map the given value to [-1, 1)
        public static double MapArcoFunc(double value)
        {
            var integer = (int)value;
            if (integer - value == 0)
                return Mod(integer, 2) == 0 ? 0.0 : -1.0;

            int floor = Convert.ToInt32(Math.Floor(value));
            int ceil = Convert.ToInt32(Math.Ceiling(value));

            return Mod(floor, 2) == 0 ? value - floor : value - ceil;
        }
    }
}
