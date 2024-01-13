using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_IP_w_UI
{
    internal class Position
    {
        public int x { get; set; }
        public int y { get; set; }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Position() { }

        public override string ToString()
        {
            return $"X: {x}, Y: {y}";
        }
    }
}
