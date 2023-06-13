using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class Level
    {
        private int levelNb;
        public int level { get { return levelNb; } set { levelNb = value; } }
        private int[][] dices;
        public int[][] Dices { get { return dices; } set { dices = value; } }
    }
}
