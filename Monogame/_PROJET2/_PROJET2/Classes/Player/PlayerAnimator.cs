using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace BricksGame
{
    public class PlayerAnimator : Animator
    {
        private Gamesystem.CharacterDirection currentDirection;
        private Player player;
        public bool startDying;
        private SoundManager soundContainer;
        private bool isBlinking = false;
        private float blinkTimer = 0f;
        private Color playerColor = Color.White;

        public PlayerAnimator(Player p_player, float p_frameTime) : base (p_frameTime)
        {
            player = p_player;
            currentDirection = Gamesystem.CharacterDirection.right;
            LoadTextures();
            ChangeState(Gamesystem.CharacterState.idle);
            soundContainer = new SoundManager(p_player);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            base.Draw(spriteBatch, position, playerColor);
        }

        public void ChangeDirection(Gamesystem.CharacterDirection direction)
        {
            if (direction == Gamesystem.CharacterDirection.left) 
            {
                ChangeState(Gamesystem.CharacterState.l_walk);
            }

            else if (direction == Gamesystem.CharacterDirection.right) 
            {
                ChangeState(Gamesystem.CharacterState.walk);
            }
            currentDirection = direction;
        }

        public void Idle()
        {
            if (currentDirection == Gamesystem.CharacterDirection.left)
            {
                ChangeState(Gamesystem.CharacterState.l_idle);
            }
            else
            {
                ChangeState(Gamesystem.CharacterState.idle);
            }
        }

        public void Action()
        {
            if (currentDirection == Gamesystem.CharacterDirection.left)
            {
                ChangeState(Gamesystem.CharacterState.l_fire);
            }

            else
            {
                ChangeState(Gamesystem.CharacterState.fire);
            }
        }

        public void Die()
        {
     
            if ((currentState != Gamesystem.CharacterState.die || currentState != Gamesystem.CharacterState.l_die) && !startDying)
            {
               SetLoop(false);
                startDying = true;
                player.StartDying();

                if (currentDirection == Gamesystem.CharacterDirection.left)
                { 
                    ChangeState(Gamesystem.CharacterState.l_die);
                }
                else
                {
                    ChangeState(Gamesystem.CharacterState.die);
                }

                soundContainer.Play(Gamesystem.CharacterState.die);
            }
        }

        public void Blink(GameTime p_GameTime, bool b)
        {
            if (b)
            {
                if (!isBlinking)
                {
                    isBlinking = true;
                    blinkTimer = 0f;
                }

                if (isBlinking)
                {
                    blinkTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;

                    if (blinkTimer >= 2f)
                    {
                        player.IsPlayerHit = false; // A changer d'endroit
                        isBlinking = false;
                        playerColor = Color.White;
                    }

                    else
                    {
                        if (blinkTimer % 1f <= 0.5f)
                        {
                            playerColor = Color.White;
                        }

                        else
                        {
                            playerColor = Color.Red;
                        }

                    }
                }
            }

            else
            {
                playerColor = Color.White;
            }
        }

        public void Reset()
        {
            playerColor = Color.White;
            ChangeState(Gamesystem.CharacterState.idle);
        }

        public void LoadTextures()
        {
            textureList = new List<Texture2D>();
            textureList.Insert((int)Gamesystem.CharacterState.idle, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad"));
            textureList.Insert((int)Gamesystem.CharacterState.l_idle, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_left"));
            textureList.Insert((int)Gamesystem.CharacterState.walk, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_walk"));
            textureList.Insert((int)Gamesystem.CharacterState.l_walk, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_walk_left"));
            textureList.Insert((int)Gamesystem.CharacterState.fire, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_attack"));
            textureList.Insert((int)Gamesystem.CharacterState.l_fire, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_attack_left"));
            textureList.Insert((int)Gamesystem.CharacterState.hit, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_walk_left"));
            textureList.Insert((int)Gamesystem.CharacterState.die, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_die"));
            textureList.Insert((int)Gamesystem.CharacterState.l_die, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/pad_die_left"));
      
            ChangeSpriteSheet(textureList[(int)Gamesystem.CharacterState.idle]);
        }

    }
}
