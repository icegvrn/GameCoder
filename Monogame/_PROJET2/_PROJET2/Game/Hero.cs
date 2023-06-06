using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class Hero : Sprite
    {
        private int Energy { get; set; }
        public Hero(List<Texture2D> pTexture) : base(pTexture)
        {
            Energy = 100;
        }

        public void TouchedBy(IActor p_By)
        {
        
        }
    }
}
