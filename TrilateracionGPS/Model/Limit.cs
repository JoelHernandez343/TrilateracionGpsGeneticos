using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrilateracionGPS.Model
{
    class Limit
    {
        private int m;

        public double A { set; get; }
        public double B { set; get; }
        public int M
        {
            set { m = value < 0 ? -value : value; }
            get => m;
        }

        public override string ToString()
        {
            return "a: " + A + " b " + B + "  m " + M;
        }


        // Generar limites
        public static Limit[] generate(Restriction[] restrictions, int n)
        {
            int i;

            double ax = 0;
            double bx = 0;

            for (i = 0; i < restrictions.Length; ++i)
            {
                if (Double.IsNaN(restrictions[i].X))
                    continue;

                //ax = restrictions[i].X;
                bx = restrictions[i].X;

                break;

            }

            for (int j = i + 1; j < restrictions.Length; ++j)
            {
                if (Double.IsNaN(restrictions[j].X))
                    continue;

                double val = restrictions[j].X;

                //if (val < ax)
                //    ax = val;
                if (val > bx)
                    bx = val;
            }


            double ay = 0;
            double by = 0;

            for (i = 0; i < restrictions.Length; ++i)
            {
                if (Double.IsNaN(restrictions[i].Y))
                    continue;

                //ay = restrictions[i].Y;
                by = restrictions[i].Y;

                break;

            }

            for (int j = i + 1; j < restrictions.Length; ++j)
            {
                if (Double.IsNaN(restrictions[j].Y))
                    continue;

                double val = restrictions[j].Y;

                //if (val < ay)
                //    ay = val;
                if (val > by)
                    by = val;
            }

            int mx = Genetics.getMj(ax, bx, n);
            int my = Genetics.getMj(ay, by, n);

            Limit[] limits = {
                new Limit() { A = ax, B = bx, M = mx },
                new Limit() { A = ay, B = by, M = my }
            };

            return limits;
        }
    }
}
