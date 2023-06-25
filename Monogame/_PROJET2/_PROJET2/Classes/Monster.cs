using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BricksGame.Classes;
using System;

namespace BricksGame
    {
        public class Monster : Bricks, ICollider, IDestroyable
        {

            public bool IsDead = false;
            private float life = 4;
            private float initialLife;
            private Animator animator;
            public bool isAttacking = false;
            private float attackTimer = 0f;
            private float attackCooldown = 2f;

        // Jauge du monstre
            private EvolutiveColoredGauge gauge;
            int lifeBarLenght = 56;
            int lifeBarHeight = 4;
            Color[] lifeColors = { Color.Green, Color.Yellow, Color.Orange, Color.Red };
            float[] threshold = { 0.65f, 0.55f, 0.35f };

        public Monster(List<Texture2D> p_textures) : base(p_textures)
            {
            BoundingBox = new Rectangle((int)(Position.X - currentTexture.Width / 2), (int)(Position.Y - currentTexture.Height / 2), currentTexture.Width, currentTexture.Height);
            CanMove = true;    
        }

            public Monster(List<Texture2D> p_textures, int Power) : base(p_textures)
            {
            life = Power * Power * 50;
            initialLife = life;
            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), (currentTexture.Width/(currentTexture.Width/currentTexture.Height)), currentTexture.Height);


            AddGauge();
           
            animator = new Animator(p_textures[0], 0.15f);
            SetSpeed(Power);
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

            if (Position.Y > ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - BoundingBox.Width*4)
            {
                CanMove = false;
                Attack();
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
        }


            public void RemoveLife(float lifeFactor)
            {
                PlayerState.AddPoints((int)lifeFactor);
                life -= lifeFactor;
                CollisionEvent = false;
            }


        public void Attack()
        {
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



