using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

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
        private List<Texture2D> textureList;
        public Gamesystem.CharacterState currentState;
        private Gamesystem.CharacterState lastState;
        private bool loop;


        public Animator(Player player, float p_frameTime)
        {
            textureList = new List<Texture2D>();
            frameTime = p_frameTime;
            loop = true;
            List<Texture2D> myPadTextureList = new List<Texture2D>();
            myPadTextureList.Insert((int)Gamesystem.CharacterState.idle, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.l_idle, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_left"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.walk, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_walk"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.l_walk, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_walk_left"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.fire, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_attack"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.l_fire, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_attack_left"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.hit, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_walk_left"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.die, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_die"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.l_die, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_die_left"));
            foreach (Texture2D texture in myPadTextureList)
            {
                textureList.Add(texture);
            }
            ChangeSpriteSheet(textureList[(int)Gamesystem.CharacterState.idle]);
        }

        public Animator(Monster monster, int lvl, float p_frameTime)
        {
            textureList = new List<Texture2D>();
            frameTime = p_frameTime;
            loop = true;
            List<Texture2D> monsterTextures = new List<Texture2D>();
            monsterTextures.Insert((int)Gamesystem.CharacterState.idle, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/idle/" + lvl + ""));
            monsterTextures.Insert((int)Gamesystem.CharacterState.l_idle, null);
            monsterTextures.Insert((int)Gamesystem.CharacterState.walk, null);
            monsterTextures.Insert((int)Gamesystem.CharacterState.l_walk, null);
            monsterTextures.Insert((int)Gamesystem.CharacterState.fire, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/attack/" + lvl + ""));
            monsterTextures.Insert((int)Gamesystem.CharacterState.l_fire, null);
            monsterTextures.Insert((int)Gamesystem.CharacterState.hit, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/hit/" + lvl + ""));
            monsterTextures.Insert((int)Gamesystem.CharacterState.die, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/die/" + lvl + ""));
            foreach (Texture2D texture in monsterTextures)
            {
                textureList.Add(texture);
            }

            ChangeSpriteSheet(textureList[(int)Gamesystem.CharacterState.idle]);
        }

        public void ChangeSpriteSheet(Texture2D p_spritesSheet)
        {

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

        public void ChangeState(Gamesystem.CharacterState state)
        {
            currentState = state;
        }

        public void Update(GameTime gameTime)
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {

            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], Color.White);

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], color);
        }

        public void SetLoop(bool p_bool)
        {
            loop = p_bool;
        }
    }
}

