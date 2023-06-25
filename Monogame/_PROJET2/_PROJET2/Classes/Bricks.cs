
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;


namespace BricksGame
{
    public class Bricks : Sprite, ICollider, IDestroyable, IBrickable
    {
        private bool isDestroy;
        public bool IsDestroy { get { return isDestroy; } set { isDestroy = value; } }

        private int life = 4;
        public bool CollisionEvent = false;
        private float timer = 0.2f;
        public Bricks(List<Texture2D> p_textures): base(p_textures) 
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);

        }

        public Bricks(List<Texture2D> p_textures, int Power) : base(p_textures)
        {
            life = Power;
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);

        }
        public void TouchedBy(GameObject p_By)
        { 
         
         
        }

      

        public override void Update(GameTime p_GameTime)
        {
            if  (life <= 0)
            {
                Destroy(this);
            }

            if (CollisionEvent)
            {
                timer -= (float)p_GameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(p_GameTime);
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {

            p_SpriteBatch.Draw(currentTexture, new Vector2(Position.X - currentTexture.Width/2, Position.Y-currentTexture.Height/2), Color.White);

            p_SpriteBatch.DrawString(AssetsManager.MainFont, life.ToString(), Position, Color.Black);
        }

    
         public void Destroy(IDestroyable brick)
        {
            isDestroy = true;
            brick = null;
        }
        public void Destroy()
        {
            Destroy(this);
        }


        
    }
}

