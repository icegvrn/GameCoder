using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace BricksGame.Classes
{
    /// <summary>
    /// La classe animator se charge de draw la bonne frame d'une spriteSheet en utilisant un timer. L'animation peut être en loop ou pas. 
    /// </summary>
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
        private bool loop;
        private Gamesystem.CharacterState lastState;
        public Gamesystem.CharacterState currentState;

        public Animator(float p_frameTime)
        {
            frameTime = p_frameTime;
            loop = true;
            currentState = Gamesystem.CharacterState.idle;
        }

        // Permet de changer la currentState de l'animator. Il se base dessus pour savoir quel spriteSheet prendre. 
        public void ChangeState(Gamesystem.CharacterState state)
        {
            currentState = state;
        }


        // Méthode permettant de changer de spriteSheet
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

        // Update de l'animator : si la state est différente de la précédente, on change de spriteSheet. Puis on update les frames par un timer qui parcours une list de rectangles. Il tient en compte si l'animation est loop ou pas.
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

        // Draw en couleur standard
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], Color.White);
        }

        // Draw avec une couleur spécifiques
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], color);
        }

        // Permet de définir si l'animation doit boucler ou s'arrêter à la dernière image via un bool
        public void SetLoop(bool p_bool)
        {
            loop = p_bool;
        }
    }
}

