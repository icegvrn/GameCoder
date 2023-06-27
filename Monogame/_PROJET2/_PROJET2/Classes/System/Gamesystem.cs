using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public static class Gamesystem
    {
        public enum dice
        {
            none,
            d3,
            d4,
            d6,
            d8,
            d10,
            d12,
            d20
        }
        public enum CharacterState { idle, l_idle, walk, l_walk, fire, die }
    }

  
}
