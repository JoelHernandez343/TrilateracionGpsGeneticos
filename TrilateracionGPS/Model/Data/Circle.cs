using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using TrilateracionGPS.Model.Helpers;

namespace TrilateracionGPS.Model.Data
{
    class CircleHashComparer : IEqualityComparer<Circle>
    {
        public bool Equals(Circle x, Circle y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.X == y.X && x.Y == y.Y && x.R == y.R;
        }

        public int GetHashCode(Circle obj)
        {
            int hash = 17;
            hash = hash * 23 + obj.X.GetHashCode();
            hash = hash * 23 + obj.Y.GetHashCode();
            hash = hash * 23 + obj.R.GetHashCode();

            return hash;
        }
    }

    public class Circle
    {
        // Properties
        public Point Center { get; set; }
        public double X
        {
            get => Center.X;
            set { Center.X = value; }
        }
        public double Y
        {
            get => Center.Y;
            set { Center.Y = value; }
        }
        private double r;
        public double R
        {
            get => r;
            set { r = value < 0 ? -value : value; }
        }

        // Constructors
        public Circle()
        {
            Center = new Point(0, 0);
        }

        // Check if the given points are contained in the circle
        public bool ContainsAll(List<Point> points)
        {
            foreach (Point p in points)
                if (!Contains(p))
                    return false;

            return true;
        }

        // Check if the point is contained in the circle
        public bool Contains(Point p) => Help.Square(p.X - X) + Help.Square(p.Y - Y) <= Help.Square(R);

        public List<Point> GetCardinalPoints()
        {
            return new List<Point>
            {
                new Point(X, Y + R),
                new Point(X, Y - R),
                new Point(X + R, Y),
                new Point(X - R, Y)
            };
        }

        /// Static methods

        // Computes the distance between two circles' centers
        public static double DistanceBetweenTwoCenters(Circle c1, Circle c2)
            => Point.DistanceBetweenTwoPoints(c1.X, c1.Y, c2.X, c2.Y);

        // Check if inner circle is enclosed in outer circle
        static bool IsEnclosedIn(Circle ic, Circle oc) => oc.R >= Circle.DistanceBetweenTwoCenters(ic, oc) + ic.R;

        // Remove all enclosing circles
        static List<Circle> RemoveEnclosingCircles(List<Circle> circles)
        {
            // De-duplicate list and order it by radius
            var orderedList = new List<Circle>(new HashSet<Circle>(circles, new CircleHashComparer())).OrderBy(circle => circle.R).ToList();
            var smallest = orderedList[0];
            orderedList.RemoveAt(0);

            // Quit all circles enclosing smallest
            var r = orderedList.Where(circle => !IsEnclosedIn(smallest, circle)).ToList();
            r.Insert(0, smallest);

            return r;
        }

        // Get the intersection points between a line and a circle
        // Throws Exception if line doesn't insersect with circle
        static List<Point> IntersectionLineCircle(Line line, Circle circle)
        {
            double cprime = line.C - line.A * circle.X - line.B * circle.Y;

            var aa = Help.Square(line.A) + Help.Square(line.B);
            Func<double, double> bb = n => -2 * n * cprime;
            Func<double, double> cc = n => cprime * cprime - Help.Square(circle.R * n);

            var (e1, e2) = Help.QuadraticEquation(aa, bb(line.A), cc(line.B));
            var (n1, n2) = Help.QuadraticEquation(aa, bb(line.B), cc(line.A));

            var x1 = e1 + circle.X;
            var x2 = e2 + circle.X;
            var y1 = n1 + circle.Y;
            var y2 = n2 + circle.Y;

            if (x1 == x2 && y1 == y2)
                return new List<Point> { new Point(x1, y2) };

            return new List<Point>
            {
                new Point(x1, y1),
                new Point(x2, y2)
            };
        }

        // Get the intersection point(s) between two circles
        // Throws Exception if the circles don't intersect themselves
        static List<Point> IntersectionCircleCircle(Circle c1, Circle c2)
        {
            var line = new Line
            {
                A = 2 * (c2.X - c1.X),
                B = 2 * (c2.Y - c1.Y),
                C = (Help.Square(c1.R) - Help.Square(c2.R)) - (Help.Square(c1.X) - Help.Square(c2.X)) - (Help.Square(c1.Y) - Help.Square(c2.Y))
            };

            return IntersectionLineCircle(line, c1);
        }

        // Calculate the insersection area of two circles
        // Throws ArgumentOutOfRangeException if the circles don't intersect themselves
        static Area GetAreaFromCircles(Circle c1, Circle c2)
        {
            try
            {
                var points = new List<Point>();

                points.AddRange(c1.GetCardinalPoints().Where(point => c2.Contains(point)).ToList());
                points.AddRange(c2.GetCardinalPoints().Where(point => c1.Contains(point)).ToList());
                points.AddRange(IntersectionCircleCircle(c1, c2));

                return Area.CalculateArea(points);
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException($"Los círculos: {{{{{c1.X}, {c1.Y}, {c1.R}}}, {{{c2.X}, {c2.Y}, {c2.R}}}}} no se intersectan en ningún punto.");
            }
        }

        // Get all intersections areas of the circles
        // Throws ArgumentOutOfRangeException if any pair of circles don't intersect themselves
        static List<Area> GetIntersectionAreas(List<Circle> circles)
        {
            var areas = new List<Area>();

            for (int i = 0; i < circles.Count; ++i)
                for (int j = i + 1; j < circles.Count; ++j)
                    areas.Add(GetAreaFromCircles(circles[i], circles[j]));

            return areas;
        }

        // Get the rectangle intersection area of circles
        // Throw Exception if a pair of circles don't intersect or doesn't exists an area of intersection
        public static Area GetIntersectionArea(List<Circle> circles)
        {
            Point leftDown, rightUp;

            var noEnclosingCircles = RemoveEnclosingCircles(circles);

            if (noEnclosingCircles.Count == 1)
            {
                var final = noEnclosingCircles[0];

                leftDown = new Point(final.X - final.R, final.Y - final.R);
                rightUp = new Point(final.X + final.R, final.Y + final.R);

                return new Area { LeftDown = leftDown, RightUp = rightUp };
            }

            var areas = GetIntersectionAreas(noEnclosingCircles);

            return Area.GetIntersectionOfAllAreas(areas);
        }
    }
}
