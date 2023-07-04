using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BricksGame.Classes;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

namespace BricksGame
{
    public class Monster : Bricks, ICollider, IDestroyable
    {
        // Etat du monstre 
        public Gamesystem.CharacterState currentState;
        public bool IsDead = false;
        public bool isAttacking = false;
        private float hitDuration = 1f;
        private float hitDurationTimer;
        private float destroyDelay = 0.8f; 
        private float destroyTimer = 0f;
        public bool startDying;


        // Vie et force 
        private float life = 4;
        private float initialLife;
        private float provisoryLife = 0;
        public int Power;

        // Jauge de vie
        private EvolutiveColoredGauge gauge;
        int lifeBarLenght = 56;
        int lifeBarHeight = 4;
        Color[] lifeColors = { new Color( 51, 225, 51 ), Color.Yellow, Color.Orange, Color.Red };
        float[] threshold = { 0.65f, 0.55f, 0.35f };

        // Animation 
        private Animator animator;

        // Attaque
        private Texture2D attackIcon;
        private float attackTimer = 0f;
        private float attackCooldown = 1.5f;

        //Dimensions
        private int monsterWidth;
        private int monsterHeight;

        //Son
        SoundManager soundContainer;



        public Monster(Texture2D p_texture, int lvl) : base(p_texture)
        {
            InitLife(lvl);
            monsterHeight = currentTexture.Height;
            monsterWidth = currentTexture.Width / (currentTexture.Width / currentTexture.Height);
            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), monsterWidth, monsterHeight);

            attackIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/icon_ennemi");
            AddGauge();

            animator = new Animator(this, lvl, 0.15f);
            SetSpeed(lvl);
            CanMove = true;
            soundContainer = new SoundManager(this, lvl);
            soundContainer.Play(Gamesystem.CharacterState.idle, 1);
        }


        public override void Update(GameTime p_GameTime)
        {

            UpdateAttackTimer(p_GameTime);
            UpdateLife(p_GameTime);
            animator.Update(p_GameTime);
            gauge.Update(p_GameTime, (int)life, new Vector2(((int)(Position.X - monsterWidth / 2)), (((int)Position.Y + monsterHeight / 2) + 2)));
            UpdateBoundingBox();
            if (currentState == Gamesystem.CharacterState.hit)
            {
                hitDurationTimer -= (float)p_GameTime.ElapsedGameTime.TotalSeconds;
                if (hitDurationTimer <= 0f)
                {
                    ChangeState(Gamesystem.CharacterState.idle); 
                }
            }
        }



        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            animator.Draw(p_SpriteBatch, new Vector2((int)(Position.X - monsterWidth / 2), (int)(Position.Y - monsterHeight / 2)));
            DrawLifeGauge(p_SpriteBatch);
          //  DrawBoundingBox(p_SpriteBatch);

        }

        public void Attack()
        {
            if (currentState != Gamesystem.CharacterState.fire)
            {
                ChangeState(Gamesystem.CharacterState.fire);
                isAttacking = true;
                attackTimer = 0f;
            } 
            else
            {
                if (attackTimer >= attackCooldown)
                {
                    isAttacking = true;
                    attackTimer = 0f;
                    soundContainer.Play(Gamesystem.CharacterState.fire, 1);
                }
                else
                {
                    isAttacking = false;
                }
            }


        }


        public void RemoveLife(float lifeFactor)
        {
            PlayerState.AddPoints((int)lifeFactor);
            provisoryLife = life - lifeFactor;
            CollisionEvent = false;
            ChangeState(Gamesystem.CharacterState.hit);
            hitDurationTimer = hitDuration;
            soundContainer.Play(Gamesystem.CharacterState.hit, 1);
            soundContainer.Play(Gamesystem.CharacterState.idle, 1);
        }


        public void ChangeState(Gamesystem.CharacterState state)
        {
            currentState = state;
            animator.ChangeState(state);
        }

        private void InitLife(int lvl)
        {
            life = lvl * 50;
            initialLife = life;
            provisoryLife = life;
            Power = lvl * 35;
        }

        private void UpdateLife(GameTime p_GameTime)
        {
            if (life > 0)
            {
                if (life != provisoryLife)
                {
                    life -= 1;
                }
            }
            else
            {
                if (currentState != Gamesystem.CharacterState.die && !IsDead)
                {
                    ChangeState(Gamesystem.CharacterState.die);
                    animator.SetLoop(false);
                    destroyTimer = 0f;
                    IsDead = true;
                    soundContainer.Play(Gamesystem.CharacterState.die, 1);

                }
                else
                {
                    destroyTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
                    if (destroyTimer >= destroyDelay)
                    {

                    
                        Destroy();
                    }
                }
               
            
             
            }
        }

        private void UpdateAttackTimer(GameTime p_GameTime)
        {

                attackTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
        
         
        }


        private void UpdateBoundingBox()
        {
            BoundingBox = new Rectangle((int)(Position.X - monsterWidth / 2), (int)(Position.Y - currentTexture.Height / 2), monsterWidth, monsterHeight);
        }


        private void SetSpeed(int lvl)
        {
            switch ((lvl - 1) / 5)
            {
                case 0:
                    Speed = 2f;
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

        public void Kill()
        {
            life = 0;
        }

        private void AddGauge()
        {
            Rectangle lifeBar = new Rectangle((int)Position.X - BoundingBox.Width / 2, (int)Position.Y, lifeBarLenght, lifeBarHeight);
            gauge = new EvolutiveColoredGauge(initialLife, lifeBar, Color.White, threshold, lifeColors, true, new Vector2(Position.X, Position.Y), Color.White);
        }

        private void DrawLifeGauge(SpriteBatch p_SpriteBatch)
        {
            gauge.Draw(p_SpriteBatch);

            if (currentState == Gamesystem.CharacterState.fire)
            {
                p_SpriteBatch.Draw(attackIcon, new Vector2((int)(Position.X), (int)(Position.Y - monsterHeight / 2)), Color.White);
            }
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



