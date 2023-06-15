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
                
            else if ((Position.X + p_x * Speed) > ServiceLocator.GetService<GraphicsDevice>().Viewport.Width - BoundingBox.Width)
            {
                Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width - BoundingBox.Width, Position.Y);
            }

            else
            {
             base.Move(p_x, p_y);
            }
            
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            DrawBoundingBox(p_SpriteBatch);
            base.Draw(p_SpriteBatch);
        }

        public void TouchedBy(GameObject p_By)
        {
            if (p_By is Ball)
            {
                Debug.WriteLine("J'ai touché la balle !");
            }

      
        }
        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            Texture2D pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixelTexture.SetData(new Color[] { Color.White });

            // Dessine les bords verticaux de la bounding box
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);

            // Dessine les bords horizontaux de la bounding box
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }
    }
}
