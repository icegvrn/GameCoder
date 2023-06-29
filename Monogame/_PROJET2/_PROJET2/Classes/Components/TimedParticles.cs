using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BricksGame
{
    public class TimedParticles : Sprite, ICollider
    {

        public float timer { get; set; }
        private float initialTimer;
        public bool isCollide { private set; get; }

        public TimedParticles(List<Texture2D> p_texture, int x, int y, int width, int height, float duration) : base(p_texture)
        {
            BoundingBox = new Rectangle(x, y, width, height);
            Position = new Vector2(x, y);
            timer = duration;
            initialTimer = timer;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void TouchedBy(GameObject p_By)
        {
                if (p_By is Monster)
                {
                    Monster brick = (Monster)p_By;
                    brick.RemoveLife(1f); 
                }
        }
        public void EndOfTouch() 
        { 
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
         p_SpriteBatch.Draw(currentTexture, Position, null, Color.Purple, 0, Vector2.Zero, new Vector2(timer/2, timer / 2), SpriteEffects.None, 1); ;

        }

        public Rectangle NextPositionX()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point((int)(Speed), 0));
            return nextPosition;
        }

        public Rectangle NextPositionY()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point(0, (int)(Speed)));
            return nextPosition;
        }
    }
}
