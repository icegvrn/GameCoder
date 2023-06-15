using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using BricksGame;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace BricksGame
    {
        public class Monster : Sprite, ICollider, IDestroyable, IBrickable
        {
            private bool isDestroy;
            public bool IsDestroy { get { return isDestroy; } set { isDestroy = value; } }
            public bool CollisionEvent = false;
            private float life = 4;
        private float initialLife;
            private float timer = 0.2f;
            private Rectangle lifeBar;
            private int lifeBarLenght = 56;
            private int lifeBarHeight = 4;
            private Rectangle currentLife;
            private Color currentLifeColor;
            private Color[] lifeColors = { Color.Green, Color.Yellow, Color.Orange, Color.Red};
        private Texture2D lifeBarTexture;

            public Monster(List<Texture2D> p_textures) : base(p_textures)
            {
            BoundingBox = new Rectangle((int)(Position.X - currentTexture.Width / 2), (int)(Position.Y - currentTexture.Height / 2), currentTexture.Width, currentTexture.Height);
        }

            public Monster(List<Texture2D> p_textures, int Power) : base(p_textures)
            {
            life = Power * Power * 50;
            initialLife = life;
            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), currentTexture.Width, currentTexture.Height);
            lifeBarTexture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/blank");
            lifeBar = new Rectangle((int)Position.X - BoundingBox.Width/2, (int)Position.Y, lifeBarLenght, lifeBarHeight);
            currentLife = new Rectangle((int)Position.X - BoundingBox.Width / 2, (int)Position.Y, (int)((life / initialLife) * lifeBarLenght), lifeBarHeight);

            }
            public void TouchedBy(GameObject p_By)
        {
            
        }
       
            public override void Update(GameTime p_GameTime)
            {
            


            lifeBar.X = (int)Position.X - BoundingBox.Width / 2;
            lifeBar.Y = ((int)Position.Y + BoundingBox.Height/2) + 2; 
            currentLife.X = lifeBar.X;
            currentLife.Y = lifeBar.Y;
            currentLife.Width = (int)((life / initialLife) * lifeBarLenght);
       

                if (life >= initialLife * 0.65f)
                {
                    currentLifeColor = lifeColors[0];
                }
                else if (life >= initialLife * 0.55f)
                {
                    currentLifeColor = lifeColors[1];
                }
                else if (life >= initialLife * 0.35f)
                {
                    currentLifeColor = lifeColors[2];
                }
                else
                {
                    currentLifeColor = lifeColors[3];
                }


                if (life <= 0)
                {
                    Destroy();
                }


            BoundingBox = new Rectangle((int)(Position.X - currentTexture.Width / 2), (int)(Position.Y - currentTexture.Height / 2), currentTexture.Width, currentTexture.Height);
         

                }

        public override void Draw(SpriteBatch p_SpriteBatch)
            {
                p_SpriteBatch.Draw(currentTexture, new Vector2((int)(Position.X - currentTexture.Width/2), (int)(Position.Y - currentTexture.Height/2)), Color.White);

                p_SpriteBatch.DrawString(AssetsManager.Font14, ((int)life).ToString(), new Vector2(Position.X, Position.Y + currentTexture.Height/1.8f), currentLifeColor);
               
                p_SpriteBatch.Draw(lifeBarTexture, lifeBar, Color.Gray);

                p_SpriteBatch.Draw(lifeBarTexture, currentLife, currentLifeColor);
             
        }


            public void Destroy()
            {
                isDestroy = true;
            }

            public void RemoveLife(float lifeFactor)
            {
                life -= lifeFactor;
                CollisionEvent = false;
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



