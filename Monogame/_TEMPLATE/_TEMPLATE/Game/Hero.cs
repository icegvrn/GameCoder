using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _TEMPLATE
{
    public class Hero : Sprite
    {
        private int Energy { get; set; }
        public Hero(List<Texture2D> pTexture) : base(pTexture)
        {
            Energy = 100;
        }

        public override void TouchedBy(IActor p_By)
        {
            base.TouchedBy(p_By);
        }
    }
}
