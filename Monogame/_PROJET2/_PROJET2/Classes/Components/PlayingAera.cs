using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class PlayingAera 
    {
        public Rectangle aera;
        public int right;
        public int bottom;

        public PlayingAera(int x , int y, int width, int height)
        {
            aera = new Rectangle(x, y, width, height);
            right = x + width;
            bottom = y + height;
        }
    }
}
