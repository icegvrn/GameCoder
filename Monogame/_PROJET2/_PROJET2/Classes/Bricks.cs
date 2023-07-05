
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;


namespace BricksGame
{
    public class Bricks : Sprite, ICollider, IDestroyable, IBrickable
    {
        private bool isDestroy;
        public bool isCollide { private set; get; }
        public float GridSlotNb { get; set; }
        public bool IsDestroy { get { return isDestroy; } set { isDestroy = value; } }

        private int life = 4;
        public bool CollisionEvent = false;
        private float timer = 0.2f;

        public Rectangle boundingBox { get { return BoundingBox; } set { BoundingBox = value; } } 

        public Bricks(Texture2D p_texture) : base(p_texture) 
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);

        }

        public Bricks(Texture2D p_texture, int Power) : base(p_texture)
        {
            life = Power;
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);

        }
        public void TouchedBy(GameObject p_By)
        { 
         
         
        }


        public void EndOfTouch()
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

            p_SpriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.MainFont), life.ToString(), Position, Color.Black);
           
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

        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            Texture2D pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);

            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
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

