using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrilateracionGPS.Model.Data;
using TrilateracionGPS.Model.Helpers;

namespace TrilateracionGPS.Model.Genetic
{
    class Limit
    {
        public double A { set; get; }
        public double B { set; get; }
        private int m;
        public int M
        {
            get => m;
            set { m = value < 0 ? -value : value; }
        }

        // Generar limites
        public static Limit[] Generate(Circle[] circles, int n, double error, Action<string> logger)
        {
            try
            {
                // Hot fix
                var original = new double[circles.Length];

                for (int i = 0; i < circles.Length; ++i) {
                    original[i] = circles[i].R;
                    circles[i].R = Math.Sqrt(Help.Square(circles[i].R) + error);
                }

                var commonArea = Circle.GetIntersectionArea(new List<Circle>(circles));

                for (int i = 0; i < circles.Length; ++i)
                    circles[i].R = original[i];

                // ---

                double ax = commonArea.LeftDown.X;
                double ay = commonArea.LeftDown.Y;

                double bx = commonArea.RightUp.X;
                double by = commonArea.RightUp.Y;

                logger($"Límites en X: ({ax}, {bx}).");
                logger($"Límites en Y: ({ay}, {by}).");

                int mx = Genetics.GetMj(ax, bx, n);
                int my = Genetics.GetMj(ay, by, n);

                Limit[] limits = {
                    new Limit() { A = ax, B = bx, M = mx },
                    new Limit() { A = ay, B = by, M = my }
                };

                return limits;
            } 
            catch (Exception e)
            {
                logger(e.Message);   
                throw e;
            }
        }
    }
}
