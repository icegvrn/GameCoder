using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BricksGame.Classes;
using System;
using System.Diagnostics;

namespace BricksGame
    {
        public class Monster : Bricks, ICollider, IDestroyable
        {

        public Gamesystem.CharacterState currentState;
        private Gamesystem.CharacterState lastState;

            public bool IsDead = false;
            private float life = 4;
            private float initialLife;
            private Animator animator;
            public bool isAttacking = false;
            private float attackTimer = 0f;
            private float attackCooldown = 7f;

        public int Power;
        private float lifeLossDelay = 0.05f; 
        private float elapsedLifeLossTime = 0f;
        private float provisoryLife = 0;

        private List<Texture2D> textures;
        private Texture2D attackIcon;

        // Jauge du monstre
        private EvolutiveColoredGauge gauge;
            int lifeBarLenght = 56;
            int lifeBarHeight = 4;
            Color[] lifeColors = { Color.Green, Color.Yellow, Color.Orange, Color.Red };
            float[] threshold = { 0.65f, 0.55f, 0.35f };



            public Monster(Texture2D p_texture, int lvl) : base(p_texture)
            {
            life = lvl * 50;
            Power = lvl* 25;
            initialLife = life;
            provisoryLife = life;
            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), (currentTexture.Width/(currentTexture.Width/currentTexture.Height)), currentTexture.Height);
      
            attackIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/icon_ennemi");
            AddGauge();
            animator = new Animator(this, lvl, 0.15f);
            SetSpeed(lvl);
            CanMove = true;
            }
         

        public void SetSpeed(int power)
        {
            switch ((power - 1) / 5)
            {
                case 0:
                    Speed = 3f;
                    break;
                case 1:
                    Speed = 2f;
                    break;
                case 2:
                    Speed = 2f;
                    break;
                case 3:
                    Speed = 1f;
                    break;

                default:
                    Speed = 1f;
                    break;
            }

        }
       
            public override void Update(GameTime p_GameTime)
        {

            attackTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
            if (life != provisoryLife)
            {
                if (life > 0)
                {
                    life -= 1; // Perte de vie d'une unité
                               // Autres actions à effectuer lorsque la vie est perdue
                }
            }

            
            animator.Update(p_GameTime);

      
            
            gauge.CurrentValue = life;
            gauge.Position = new Vector2(((int)(Position.X - (currentTexture.Width / (currentTexture.Width / currentTexture.Height)) / 2)), (((int)Position.Y + BoundingBox.Height / 2) + 2));
            gauge.Update(p_GameTime);
     

                if (life <= 0)
                {
                IsDead = true;
                    Destroy();
                }


            BoundingBox = new Rectangle((int)(Position.X - (currentTexture.Width / (currentTexture.Width / currentTexture.Height)) / 2), (int)(Position.Y - currentTexture.Height / 2), (currentTexture.Width / (currentTexture.Width / currentTexture.Height)), currentTexture.Height);
         

                }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            animator.Draw(p_SpriteBatch, new Vector2((int)(Position.X - (currentTexture.Width / (currentTexture.Width / currentTexture.Height)) / 2), (int)(Position.Y - currentTexture.Height / 2)));
            gauge.Draw(p_SpriteBatch);

            if (currentState == Gamesystem.CharacterState.fire)
            {
                p_SpriteBatch.Draw(attackIcon, new Vector2((int)(Position.X), (int)(Position.Y - currentTexture.Height / 2)), Color.White);
            }

            DrawBoundingBox(p_SpriteBatch);

        }


        public void RemoveLife(float lifeFactor)
            {
                PlayerState.AddPoints((int)lifeFactor);
                provisoryLife = life - lifeFactor;
                CollisionEvent = false;
                elapsedLifeLossTime = 0f; 

        }

        public void ChangeState(Gamesystem.CharacterState state)
        {
            currentState = state;
            animator.ChangeState(state);
        }

        public void Attack()
        {
            if (currentState != Gamesystem.CharacterState.fire)
            {
                ChangeState(Gamesystem.CharacterState.fire);
            }
          
            if (attackTimer >= attackCooldown)
            {
                isAttacking = true;
                attackTimer = 0f;
            
            }
            else
            {
                isAttacking = false;
              
            }
           
        }

   

        public void AddGauge()
        {
            Rectangle lifeBar = new Rectangle((int)Position.X - BoundingBox.Width / 2, (int)Position.Y, lifeBarLenght, lifeBarHeight); 
            gauge = new EvolutiveColoredGauge(initialLife, lifeBar, Color.White, threshold, lifeColors, true, new Vector2(Position.X, Position.Y), Color.White);
        }


        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            Texture2D boxTexture = AssetsManager.blankTexture;
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);


            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }



    }


    }



