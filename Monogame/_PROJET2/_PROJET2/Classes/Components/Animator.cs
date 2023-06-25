using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace BricksGame.Classes
{
    public  class Animator
    {
            private Rectangle[] frames;
            private Texture2D spriteSheet;
            private int currentFrame;
            private float frameTime;
            private float timer;
            private int frameNb;
        private int frameWidth;
        private int frameHeight;

            public Animator(Texture2D p_spritesSheet, float p_frameTime)
            {
            frameTime = p_frameTime;
            ChangeSpriteSheet(p_spritesSheet);
        }

        public void ChangeSpriteSheet(Texture2D p_spritesSheet) {
            if (p_spritesSheet != spriteSheet)
            {
                spriteSheet = p_spritesSheet;
                currentFrame = 0;
                timer = 0f;
                frameNb = spriteSheet.Width / spriteSheet.Height;
                frameWidth = spriteSheet.Width / frameNb; // Largeur d'une frame
                frameHeight = spriteSheet.Height; // Hauteur de la spritesheet
                frames = new Rectangle[frameNb];

                for (int i = 0; i < frameNb; i++)
                {
                    frames[i] = new Rectangle(i * frameWidth, 0, frameWidth, frameHeight);
                }
            }
           

        }

            public void Update(GameTime gameTime)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timer >= frameTime)
                {
                    currentFrame++;
                    if (currentFrame >= frames.Length)
                    {
                        currentFrame = 0;
                    }
                    timer = 0f;
                }
            }

            public void Draw(SpriteBatch spriteBatch, Vector2 position)
            {
                spriteBatch.Draw(spriteSheet, position, frames[currentFrame], Color.White);
            }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], color);
        }
    }
    }

