using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrilateracionGPS.Model
{
    class Restriction
    {
        public Func<double, double, bool> condition;
        public double X { get; }
        public double Y { get; }

        public Restriction(Circle circle, double error)
        {
            condition = createCircleRestriction(circle, error);

            X = Math.Sqrt(error + square(circle.R) - square(circle.Y)) + circle.X; // Retornara NaN si hay raiz negativa
            Y = Math.Sqrt(error + square(circle.R) - square(circle.X)) + circle.Y; // Retornara NaN si hay raiz negativa

        }

        // Cosas estaticas
        public static Func<double, double, double> z = null;

        public static void initializeZ(Circle[] circles)
        {
            z = (double x, double y) => {
                double result = 0;

                Circle[] c = new Circle[circles.Length];
                Array.Copy(circles, c, circles.Length);

                for (int i = 0; i < c.Length; ++i)
                {
                    result += square(square(x - c[i].X) + square(y - c[i].Y) - square(c[i].R));
                }

                return result;
            };
        }

        // Cuadrado simplificado
        static double square(double a) => a * a;
        // Crea una nueva funcion con los parametros dados
        static Func<double, double, bool> createCircleRestriction(Circle circle, double error)
        {
            return (double x, double y) => {
                //Console.WriteLine(x + " " + y + " - " + (square(x - circle.X) + square(y - circle.Y) - square(circle.R)) + " e: " + error);
                return square(x - circle.X) + square(y - circle.Y) - square(circle.R) <= error;
            };
        }

        public static double getAbsoluteError(double e, int numOfRestrictions) => Math.Sqrt(z(0, 0)) * e / numOfRestrictions;

        public static double getRelativeError(double e, int numOfRestrictions) => z(0, 0) * e / numOfRestrictions;

        // Generar restricciones
        public static Restriction[] generate(Circle[] circles, double e, bool rel)
        {
            double error = rel ? Restriction.getRelativeError(e, circles.Length) : Restriction.getAbsoluteError(e, circles.Length);
            Console.WriteLine(error);

            Restriction[] restrictions = new Restriction[circles.Length];
            for (int i = 0; i < restrictions.Length; ++i)
                restrictions[i] = new Restriction(circles[i], error);

            return restrictions;
        }

    }
}
