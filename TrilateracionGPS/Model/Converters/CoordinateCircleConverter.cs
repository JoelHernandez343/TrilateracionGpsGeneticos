using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrilateracionGPS.Model.Data;
using TrilateracionGPS.Model.Helpers;

namespace TrilateracionGPS.Model.Converters
{
    class CoordinateCircleConverter
    {
        static readonly double circunference = 40030.174;

        public static Circle CoordinateToCircle(Coordinate coordinate)
        {
            double y = circunference / 360 * Math.Cos(coordinate.Latitude * Math.PI / 180);
            double x = y * coordinate.Longitude;

            return new Circle { X = x, Y = y, R = coordinate.Distance };
        }

        public static Coordinate CircleToCoordinate(Circle circle) => new Coordinate
        {
            Latitude = (180 / Math.PI) * Math.Acos(circle.Y * 360 / circunference),
            Longitude = circle.X / circle.Y,
            Distance = circle.R
        };
    }
}
