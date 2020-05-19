using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrilateracionGPS.Model
{
    public struct Pair
    {
        public char[] Item { get; set; }
        public int Repetitions { get; set; }
        public double Weight { get; set; }

        public bool EqualsTo(char[] item)
        {
            if (Item.Length != item.Length) return false;

            for (int i = 0; i < item.Length; ++i)
                if (Item[i] != item[i])
                    return false;

            return true;
        }
    }
}
