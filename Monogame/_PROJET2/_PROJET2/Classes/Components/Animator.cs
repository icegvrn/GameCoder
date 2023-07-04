using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace BricksGame.Classes
{
    public class Animator
    {
        private Rectangle[] frames;
        private Texture2D spriteSheet;
        private int currentFrame;
        private float frameTime;
        private float timer;
        private int frameNb;
        private int frameWidth;
        private int frameHeight;
        protected List<Texture2D> textureList;
        public Gamesystem.CharacterState currentState;
        private Gamesystem.CharacterState lastState;
        private bool loop;

        public Animator(float p_frameTime)
        {
            frameTime = p_frameTime;
            loop = true;
            currentState = Gamesystem.CharacterState.idle;
        }

        public void ChangeSpriteSheet(Texture2D p_spritesSheet)
        {
            if (p_spritesSheet != spriteSheet)
            {
                spriteSheet = p_spritesSheet;
                currentFrame = 0;
                timer = 0f;
                frameNb = spriteSheet.Width / spriteSheet.Height;
                frameWidth = spriteSheet.Width / frameNb; 
                frameHeight = spriteSheet.Height; 
                frames = new Rectangle[frameNb];

                for (int i = 0; i < frameNb; i++)
                {
                    frames[i] = new Rectangle(i * frameWidth, 0, frameWidth, frameHeight);
                }
            }


        }

        public void ChangeState(Gamesystem.CharacterState state)
        {
            currentState = state;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (lastState != currentState)
            {
                ChangeSpriteSheet(textureList[(int)currentState]);
                lastState = currentState;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= frameTime)
            {
                currentFrame++;

                if (currentFrame >= frames.Length)
                {
                    if (loop)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        currentFrame = frames.Length-1;
                    }
                }

                timer = 0f;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {

            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], Color.White);

        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], color);
        }

        public void SetLoop(bool p_bool)
        {
            loop = p_bool;
        }
    }
}

