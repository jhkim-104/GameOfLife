using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    [Serializable]
    class Cel
    {
        public static int size = 10;
        public int isLive = 0;
        public bool next = false;

        public Cel()
        {
            isLive = 1;
        }

        
        static public bool State(Dictionary<Point, Cel> cels, Point pt)
        {
            if (cels.ContainsKey(pt) == true)
            {
                if (cels[pt].isLive > 0)
                    return true;
            }
            return false;
        }
    }
}
