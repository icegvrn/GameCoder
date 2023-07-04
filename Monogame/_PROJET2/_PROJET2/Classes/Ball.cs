using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace BricksGame
{
    public class Ball : Sprite, ICollider, IDestroyable
    {

        public Power power;
        private int strenght;
        private int initialStrenght;

        // Etats de la balle
        private Gamesystem.BallState currentSate;

        public bool isFired;
        public bool IsDestroy { get; set; }

        // Direction de la balle
        private double angle;
        public Vector2 SpeedVector;

        // Curseur de lancé de la balle
        private Texture2D lineTexture;
        private double distanceFromMouse;

        // Particules de la balle
        public List<TimedParticles> timedParticles { get; set; }

        // Son
        private bool hit;
        private float hitSoundDelay = 1f;
        private float hitTimer = 0f;
        private SoundManager soundContainer;


        public Ball(List<Texture2D> p_texture) : base(p_texture)
        {
            timedParticles = new List<TimedParticles>();
            lineTexture = AssetsManager.blankTexture;
            CanMove = false;
            Speed = 10f;
            SpeedVector = new Vector2(Speed, -Speed);
            currentSate = Gamesystem.BallState.idle;
            soundContainer= new SoundManager(this);
            power = null;
            strenght = 50;
            initialStrenght = 50;
        }

        public override void Update(GameTime p_GameTime)
        {
            if (isFired)
            {
                CanMove = true;
                AddParticles(p_GameTime); 
                TryMove();
            }
            else
            {
                UpdatePositionIfFollowingSomething();
            }

            if (hit)
            {
                hitTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
                if (hitTimer >= hitSoundDelay)
                {
                    currentSate = Gamesystem.BallState.fired;
                    hit = false;
                    hitTimer = 0;
                }
            }

            if (power is not null && power.powerCharged)
            {
                foreach (TimedParticles particle in timedParticles)
                {
                    particle.currentColor = particle.colors[1];
                }
            }
            else
            {
                foreach (TimedParticles particle in timedParticles)
                {
                    particle.currentColor = particle.colors[0];
                }
            }
            if (power != null && power is DamagePower)
            {
                DamagePower dmgPower = ((DamagePower)power);

               if (dmgPower.powerIsOn)
                {
                    strenght = initialStrenght * dmgPower.multiplicator;
                }
               else
                {
                    strenght = initialStrenght;
                }

                dmgPower.Update(p_GameTime);
            }

            UpdateParticles(p_GameTime);
            base.Update(p_GameTime);
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if ((!isFired) && (ServiceLocator.GetService<LevelManager>().currentState == LevelManager.LevelState.play))
            {
                DrawTrajectory(p_SpriteBatch);
            }

            base.Draw(p_SpriteBatch);

            if (power != null && power is DamagePower)
            {
                DamagePower dmgPower = ((DamagePower)power);
                if (dmgPower.powerIsOn)
                {
                    p_SpriteBatch.DrawString(AssetsManager.MainFont, ((int)dmgPower.timer).ToString(), new Vector2(Position.X - 10, Position.Y - 10), Color.Red);
                } 
            }
        }

    
        public void CheckCollision(List<IBrickable> brick)
        {
            for (int i = 0; i < brick.Count; i++)
            {
                if (brick[i] is Monster && !((Bricks)brick[i]).IsDestroy)
                {
                    Monster c_Brick = (Monster)brick[i];
                    if (c_Brick.BoundingBox.Intersects(NextPositionY()))
                    {
                        InverseVerticalDirection();
                        c_Brick.RemoveLife(strenght);
                        if (power is not null && power.powerCharged)
                        {
                            power.Trigger(c_Brick, ServiceLocator.GetService<LevelManager>().GameGrid);
                        }
                        
                    }

                    if (c_Brick.BoundingBox.Intersects(NextPositionX()))
                    {
                        InverseHorizontalDirection();
                        c_Brick.RemoveLife(strenght);

                        if (power is not null && power.powerCharged)
                        {
                            power.Trigger(c_Brick, ServiceLocator.GetService<LevelManager>().GameGrid);
                        }

                    }

                }

            }

        }

        public Rectangle NextPositionX()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point((int)SpeedVector.X, 0));
            return nextPosition;
        }

        public Rectangle NextPositionY()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point(0, (int)SpeedVector.Y));
            return nextPosition;
        }

        public void CheckCollision(Player player)
        {
            float relativePositionX = Position.X - player.Position.X;
            float padDivideBySix = player.BoundingBox.Width / 6f;

            if (player.BoundingBox.Intersects(NextPositionX()) || player.BoundingBox.Intersects(NextPositionY()))
            {
                if (relativePositionX < padDivideBySix)
                {
                    angle = MathHelper.ToRadians(20f);
                    SpeedVector.X = -(float)Math.Cos(angle) * Speed;
                    SpeedVector.Y = -(float)Math.Sin(angle) * Speed;

                }
                else if (relativePositionX < 2 * padDivideBySix)
                {
                    angle = MathHelper.ToRadians(35f);
                    SpeedVector.X = -(float)Math.Cos(angle) * Speed;
                    SpeedVector.Y = -(float)Math.Sin(angle) * Speed;

                }

                else if (relativePositionX < 3 * padDivideBySix)
                {
                    angle = MathHelper.ToRadians(75f);
                    SpeedVector.X = (float)Math.Cos(angle) * Speed;
                    SpeedVector.Y = -(float)Math.Sin(angle) * Speed;

                }

                else if (relativePositionX < 4 * padDivideBySix)
                {
                    angle = MathHelper.ToRadians(75f);
                    SpeedVector.X = (float)Math.Cos(angle) * Speed;
                    SpeedVector.Y = -(float)Math.Sin(angle) * Speed;

                }

                else if (relativePositionX < 5 * padDivideBySix)
                {
                    angle = MathHelper.ToRadians(35f);
                    SpeedVector.X = (float)Math.Cos(angle) * Speed;
                    SpeedVector.Y = -(float)Math.Sin(angle) * Speed;
                }

                else
                {
                    angle = MathHelper.ToRadians(20f);
                    SpeedVector.X = (float)Math.Cos(angle) * Speed;
                    SpeedVector.Y = -(float)Math.Sin(angle) * Speed;
                }
                soundContainer.Play(Gamesystem.BallState.fired);

            }
        }
      
        public void TryMove()
        {
            ClampScreenPosition();
            Move(SpeedVector);
        }

        public void Move(Vector2 vitesse)
        {
            Position = new Vector2(Position.X + vitesse.X, Position.Y + vitesse.Y);
        }

        public void InverseHorizontalDirection()
        {
            SpeedVector = new Vector2(-SpeedVector.X, SpeedVector.Y);
            HitEvents();
        }

        public void InverseVerticalDirection()
        {
            SpeedVector = new Vector2(SpeedVector.X, -SpeedVector.Y);
            HitEvents();
        }

        public void Fire()
        {
            if (!isFired)
            {
                ObjectToFollow = null;
                CalcTrajectory();
                CanMove = true;
                isFired = true;
                currentSate = Gamesystem.BallState.fired;
                soundContainer.Play(Gamesystem.BallState.fired);
            }

        }

        private void ClampScreenPosition()
        {
            if (Position.X + currentTexture.Width > ServiceLocator.GetService<PlayerArea>().area.Right)
            {
                Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.Right - BoundingBox.Width, Position.Y);
                InverseHorizontalDirection();
            }

            if (Position.X < ServiceLocator.GetService<PlayerArea>().area.X)
            {
                Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.X, Position.Y);

                InverseHorizontalDirection();
            }

            if (Position.Y < ServiceLocator.GetService<PlayerArea>().area.Y)
            {
                Position = new Vector2(Position.X, ServiceLocator.GetService<PlayerArea>().area.Y);

                InverseVerticalDirection();
            }

            if (Position.Y > ServiceLocator.GetService<GraphicsDevice>().Viewport.Height * 1.5f)
            {

                Destroy(this);

            }

            if (Position.Y < 0)
            {
                Destroy(this);
            }
        }


        public void AddParticles(GameTime p_GameTime)
        {
            List<Texture2D> textures = new List<Texture2D> { (ServiceLocator.GetService<ContentManager>()).Load<Texture2D>("images/round") };
            TimedParticles particle = new TimedParticles(textures, (int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height, 1.3f);
            timedParticles.Add(particle);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(particle);
        }

        private void UpdateParticles(GameTime p_GameTime)
        {
            for (int n = timedParticles.Count() - 1; n >= 0; n--)
            {
                if (timedParticles[n].timer <= 0)
                {
                    timedParticles.Remove(timedParticles[n]);
                    ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(timedParticles[n]);
                }
                else
                {
                    timedParticles[n].timer -= (float)p_GameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public void CalcTrajectory()
        {
            MouseState mouse = ServiceLocator.GetService<MouseState>();
            distanceFromMouse = Utils.calcDistance(Position.X, Position.Y, mouse.X, mouse.Y);
            angle = Utils.calcAngleWithMouse(Position.X, Position.Y);
            SpeedVector = new Vector2(((float)Math.Cos(angle) * Speed), (float)Math.Sin(angle) * Speed);
        }

        private void HitEvents()
        {
            currentSate = Gamesystem.BallState.hit;
            hit = true;
            soundContainer.Play(Gamesystem.BallState.hit);
        }

        public void DrawTrajectory(SpriteBatch p_SpriteBatch)
        {
            CalcTrajectory();
            Rectangle line = new Rectangle((int)Position.X + currentTexture.Width / 2, (int)Position.Y + currentTexture.Height / 2, (int)distanceFromMouse, 2);
            Rectangle reflectLine = new Rectangle((int)Position.X + currentTexture.Width / 2, (int)Position.Y + currentTexture.Height / 2, (int)distanceFromMouse, 2);
            p_SpriteBatch.Draw(lineTexture, line, line, Color.CornflowerBlue, (float)angle, Vector2.Zero, SpriteEffects.None, 1);

        }

        public void Destroy(IDestroyable ball)
        {
            for (int n = timedParticles.Count() - 1; n >= 0; n--)
            {
                ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(timedParticles[n]);
                timedParticles.Remove(timedParticles[n]);
            }

            ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(this);
            currentSate = Gamesystem.BallState.destroyed;
            soundContainer.Play(Gamesystem.BallState.destroyed);
            IsDestroy = true;
            ball = null;
        }

        public void Destroy()
        {
            Destroy(this);
            soundContainer.Play(Gamesystem.BallState.destroyed);
        }


        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }

        public void TouchedBy(GameObject p_By)
        {
//
        }

        public void ActivePower()
        {
            power.Activate();
        }

        public void TriggerPower()
        {
            if (power != null)
            {
                power.powerUsed = true;
            }
        

        }

        public void SetSpeed(float speed)
        {
            SpeedVector = new Vector2(speed, -speed);
        }


    }



}