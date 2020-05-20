using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrilateracionGPS.Model.Helpers
{
    
    public class PriorityQueue
    {
        public List<Pair> queue;

        // Get each Pair.Item of each queue element and return them
        public char[][] GetOnlyValues()
        {
            char[][] result = new char[queue.Count][];

            for (int i = 0; i < result.Length; ++i)
                result[i] = queue[i].Item;

            return result;
        }

        // Constructor
        public PriorityQueue()
        {
            queue = new List<Pair>();
        }
        
        // Push an element, ordering first by repetitions, then by weight
        public void Push(char[] item, double weight)
        {
            // First element to be inserted
            if (queue.Count == 0)
            {
                queue.Add(new Pair { Item = item, Repetitions = 1, Weight = weight});
                return;
            }

            // Search the element in the queque
            int i = SearchFor(item);

            // If exits, update the repetitions or add a new element if not
            if (i >= 0)
                queue[i] = new Pair { Item = queue[i].Item, Repetitions = queue[i].Repetitions + 1, Weight = queue[i].Weight };
            else
                queue.Add(new Pair { Item = item, Repetitions = 1, Weight = weight });

            Shift(i);

        }

        // Search for the item inside the queue and returns its index
        // Return -1 if not
        public int SearchFor(char[] item) => queue.FindIndex(e => e.EqualsTo(item));

        // Shift the queue starting from i
        void Shift(int i)
        {
            for (int j = i - 1; j >= 0; --j)
            {
                if (queue[i].Repetitions < queue[j].Repetitions)
                    break;
                
                if (queue[i].Repetitions > queue[j].Repetitions)
                {
                    Swap(i, j);
                    i = j;
                    continue;
                }

                if (queue[i].Weight <= queue[j].Weight)
                    continue;

                Swap(i, j);
                i = j;
            }
        }

        // Swap i with j elements in the queue
        void Swap(int i, int j)
        {
            Pair aux = queue[i];
            queue[i] = queue[j];
            queue[j] = aux;
        }

    }
}
