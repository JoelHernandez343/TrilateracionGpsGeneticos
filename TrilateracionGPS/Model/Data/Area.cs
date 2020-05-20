using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrilateracionGPS.Model.Data
{
    public class Area
    {
        public Point LeftDown { get; set; }
        public Point RightUp { get; set; }

        /// Static methods
        
        // Calculate a rectangular area that enclose all the points
        public static Area CalculateArea(List<Point> points)
        {
            double minX = points[0].X;
            double maxX = points[0].X;

            double minY = points[0].Y;
            double maxY = points[0].Y;

            foreach (var p in points)
            {
                if (minX > p.X)
                    minX = p.X;

                if (maxX < p.X)
                    maxX = p.X;

                if (minY > p.Y)
                    minY = p.Y;

                if (maxY < p.Y)
                    maxY = p.Y;
            }

            return new Area
            {
                LeftDown = new Point(minX, minY),
                RightUp = new Point(maxX, maxY)
            };
        }

        // Get the limits of the intersection in an axis
        // Throws Exception if there are not intersection
        static (double, double) GetLimitsOfAxis(List<Area> areas, string axis)
        {
            var end = axis == "X" ? areas[0].RightUp.X : areas[0].RightUp.Y;
            for (int i = 1; i < areas.Count; ++i)
            {
                var aux = axis == "X" ? areas[i].RightUp.X : areas[i].RightUp.Y;
                if (aux < end)
                    end = aux;
            }

            var begin = axis == "X" ? areas[0].LeftDown.X : areas[0].LeftDown.Y;
            for (int i = 1; i < areas.Count; ++i)
            {
                var aux = axis == "X" ? areas[i].LeftDown.X : areas[i].LeftDown.Y;
                if (aux > begin)
                    begin = aux;
            }

            if (begin >= end)
                throw new Exception($"No existe una intersección común en las áreas a lo largo de {axis}");

            return (begin, end);
        }

        // Get the intersection of all areas
        // Throw Exception if there are not intersection
        public static Area GetIntersectionOfAllAreas(List<Area> areas)
        {
            var (xBegin, xEnd) = GetLimitsOfAxis(areas, "X");
            var (yBegin, yEnd) = GetLimitsOfAxis(areas, "Y");

            return new Area
            {
                LeftDown = new Point(xBegin, yBegin),
                RightUp = new Point(xEnd, yEnd)
            };
        }
    }
}
