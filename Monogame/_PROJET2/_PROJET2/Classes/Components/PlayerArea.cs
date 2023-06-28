using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class PlayerArea 
    {
        public Rectangle area;
        public int right;
        public int bottom;

        public PlayerArea(int x , int y, int width, int height)
        {
            area = new Rectangle(x, y, width, height);
            right = x + width;
            bottom = y + height;
        }
    }
}
