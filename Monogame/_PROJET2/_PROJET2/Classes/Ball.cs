using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class Ball : Sprite, ICollider
    {
        public Ball(List<Texture2D> p_texture) : base(p_texture)
        {

        }
        public void TouchedBy(IActor p_By)
        {
          
        }

        public override void Update(GameTime p_GameTime)
        {
            Move(0, -2);
            base.Update(p_GameTime);
        }

    }
}
