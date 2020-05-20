using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrilateracionGPS.Model.Data
{
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private double distance;
        public double Distance
        {
            get => distance;
            set
            {
                distance = value < 0.0 ? -value : value;
            }
        }

        public Coordinate() { Latitude = 0; Longitude = 0; Distance = 0; }
        public Coordinate(double latitude, double longitude, double distance) { Latitude = latitude; Longitude = longitude; Distance = distance; }

    }
}
