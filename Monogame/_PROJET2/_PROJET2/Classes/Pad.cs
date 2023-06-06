using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class Pad : Sprite, ICollider
    {
        private float _speed = 15;
        public override float Speed { get { return _speed; } set { _speed = value; } }
        public Pad(List<Texture2D> p_texture) : base(p_texture)
        {
            Speed = 15;
        }

        public override void Update(GameTime p_GameTime)
        {
                    base.Update(p_GameTime);
        }

        public override void Move(float p_x, float p_y)
        {
            if (Position.X + p_x * Speed < 0) 
            {
                Position = new Vector2(0, Position.Y);
            }
                
            else if ((Position.X + p_x * Speed) > 800 - BoundingBox.Width)
            {
                Position = new Vector2(800-BoundingBox.Width, Position.Y);
            }

            else
            {
             base.Move(p_x, p_y);
            }
            
        }


        public void TouchedBy(IActor p_By)
        {
            if (p_By is Ball)
            {
                Debug.WriteLine("J'ai touché la balle !");
            }

      
        }

    }
}
