using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BricksGame
{
    /// <summary>
    /// Classe héritante d'Animator qui permet d'animer le joueur. Elle contient les textures mais aussi le comportement particulier du joueur : change de direction, marcher, idle etc.
    /// </summary>
    public class PlayerAnimator : Animator
    {
        private Player player;

        //Etat de l'animator joueur
        private Gamesystem.CharacterDirection currentDirection;
        private bool isBlinking = false;
        private float blinkTimer = 0f;
        private Color playerColor = Color.White;
        public bool StartDying { get; private set; }

        //Son
        private MediaPlayerService soundContainer;

        public PlayerAnimator(Player p_player, float p_frameTime) : base (p_frameTime)
        {
            player = p_player;
            currentDirection = Gamesystem.CharacterDirection.right;
            LoadTextures();
            ChangeState(Gamesystem.CharacterState.idle);
            soundContainer = new MediaPlayerService(p_player);
        }

        // Dessin du personnage joueur
        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            base.Draw(spriteBatch, position, playerColor);
        }

        // Méthode permettant d'indiquer dans quelle direction se trouve le personnage
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


        // Méthode permettant de jouer l'animation IDLE du joueur
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

        // Méthode permettant de jouer l'animation d'action du personnage
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

        // Méthode permettant de jouer l'animation de mort du personnage
        public void Die()
        {
            if ((currentState != Gamesystem.CharacterState.die || currentState != Gamesystem.CharacterState.l_die) && !StartDying)
            {
                SetLoop(false);
                StartDying = true;
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


        // Méthode permettant de faire clignoter le joueur en rouge (utilisé pour les phases où il est blessé)
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
                        player.OnHit(false);
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

        // Reset de l'animation du joueur : on repasse en IDLE et en couleur standard.
        public void Reset()
        {
            playerColor = Color.White;
            ChangeState(Gamesystem.CharacterState.idle);
        }

        // Chargement des textures pour le joueur
        public void LoadTextures()
        {
            ContentManager content = ServiceLocator.GetService<ContentManager>();
            string imgPath = ServiceLocator.GetService<IPathsService>().GetPlayerImagesPathRoot();
            textureList = new List<Texture2D>();
            textureList.Insert((int)Gamesystem.CharacterState.idle, content.Load<Texture2D>(imgPath+"pad"));
            textureList.Insert((int)Gamesystem.CharacterState.l_idle, content.Load<Texture2D>(imgPath + "pad_left"));
            textureList.Insert((int)Gamesystem.CharacterState.walk, content.Load<Texture2D>(imgPath + "pad_walk"));
            textureList.Insert((int)Gamesystem.CharacterState.l_walk, content.Load<Texture2D>(imgPath + "pad_walk_left"));
            textureList.Insert((int)Gamesystem.CharacterState.fire, content.Load<Texture2D>(imgPath + "pad_attack"));
            textureList.Insert((int)Gamesystem.CharacterState.l_fire, content.Load<Texture2D>(imgPath + "pad_attack_left"));
            textureList.Insert((int)Gamesystem.CharacterState.hit, content.Load<Texture2D>(imgPath + "pad_walk_left"));
            textureList.Insert((int)Gamesystem.CharacterState.die, content.Load<Texture2D>(imgPath + "pad_die"));
            textureList.Insert((int)Gamesystem.CharacterState.l_die, content.Load<Texture2D>(imgPath + "pad_die_left"));
      
            ChangeSpriteSheet(textureList[(int)Gamesystem.CharacterState.idle]);
        }

    }
}
