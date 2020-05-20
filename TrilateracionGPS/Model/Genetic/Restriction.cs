using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrilateracionGPS.Model.Data;
using TrilateracionGPS.Model.Helpers;

namespace TrilateracionGPS.Model.Genetic
{
    class Restriction
    {
        // Represents the objetive function
        public static Func<double, double, double> Z = null;
        
        // Create a new objetive function
        public static void InitializeZ(Circle[] circles)
        {
            Z = (double x, double y) => {
                double result = 0;

                foreach(var circle in circles)
                    result += Help.Square(Help.Square(x - circle.X) + Help.Square(y - circle.Y) - Help.Square(circle.R));

                return result;
            };
        }

        // Create a new restriction
        static Func<double, double, bool> CreateCircleRestriction(Circle circle, double error) =>
            (double x, double y) => Help.Square(x - circle.X) + Help.Square(y - circle.Y) - Help.Square(circle.R) <= error;

        // Error calculation (Modified to meters)
        public static double AbsoluteError(double e, int numOfRestrictions) => Math.Sqrt(Z(0, 0)) * e / (numOfRestrictions * 10E+6);
        public static double RelativeError(double e, int numOfRestrictions) => Z(0, 0) * e / (numOfRestrictions * 10E+6);
        public static double CalculateError(double e, int length,bool rel) => rel ? RelativeError(e, length) : AbsoluteError(e, length);

        // Generate all restrictions
        public static Func<double, double, bool>[] Generate(Circle[] circles, double error)
        { 
            var restrictions = new Func<double, double, bool>[circles.Length];
            for (int i = 0; i < restrictions.Length; ++i)
            {
                restrictions[i] = CreateCircleRestriction(circles[i], error);
            }

            return restrictions;
        }

    }
}
