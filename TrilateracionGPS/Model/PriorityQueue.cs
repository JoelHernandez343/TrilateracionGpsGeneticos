using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrilateracionGPS.Model
{
    public struct Pair
    {
        public char[] item
        {
            get; set;
        }
        public int repetitions
        {
            get; set;
        }
        public double weight
        {
            get; set;
        }

        public bool equalTo(char[] item)
        {
            if (this.item.Length != item.Length)
                return false;

            for (int i = 0; i < item.Length; ++i)
                if (this.item[i] != item[i])
                    return false;

            return true;
        }


    }
    public class PriorityQueue
    {
        public List<Pair> queue;

        public char[][] getOnlyValues()
        {

            char[][] result = new char[queue.Count][];

            for (int i = 0; i < result.Length; ++i)
                result[i] = queue[i].item;

            return result;
        }

        public PriorityQueue()
        {
            queue = new List<Pair>();
        }

        public void push(char[] item, double weight)
        {

            if (queue.Count == 0)
            {
                queue.Add(new Pair
                {
                    item = item,
                    repetitions = 1,
                    weight = weight
                });
                return;
            }

            bool find = false;

            int i;
            for (i = queue.Count - 1; i >= 0; --i)
            {
                if (!queue[i].equalTo(item))
                    continue;

                queue[i] = new Pair
                {
                    item = queue[i].item,
                    repetitions = queue[i].repetitions + 1,
                    weight = queue[i].weight
                };
                find = true;
                break;
            }

            if (!find)
            {
                queue.Add(new Pair
                {
                    item = item,
                    repetitions = 1,
                    weight = weight
                });
                i = queue.Count - 1;
            }

            for (int j = i - 1; j >= 0; --j)
            {

                if (queue[i].repetitions < queue[j].repetitions)
                    break;

                if (queue[i].repetitions > queue[j].repetitions)
                {
                    Pair aux = queue[i];
                    queue[i] = queue[j];
                    queue[j] = aux;
                    i = j;
                    continue;
                }

                if (queue[i].weight > queue[j].weight)
                {
                    Pair aux = queue[i];
                    queue[i] = queue[j];
                    queue[j] = aux;
                    i = j;
                }
            }
        }

    }
}
