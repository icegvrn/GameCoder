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
        private Texture2D lineTexture;
        public List<TimedParticles> timedParticles { get; set; }
        private double angle;
        private double distanceFromMouse;
        private Vector2 destination;
        private bool isFired;
        public bool collide = false;
        public bool IsDestroy { get; set; }
        public bool CollisionEvent = false;
        private float timer = 0.2f;

        public Vector2 Vitesse;

        public Ball(List<Texture2D> p_texture) : base(p_texture)
        {
            timedParticles = new List<TimedParticles>();
            lineTexture = AssetsManager.blankTexture;
            Speed = 10f;
            CanMove = false;
            Vitesse = new Vector2(6, -6);

        }


        public void TouchedBy(GameObject p_By)
        {

        }

        public void CheckCollision(List<IBrickable> brick)
        {
            for (int i = 0; i < brick.Count; i++)
            {
                if (brick[i] is Monster && !((Bricks)brick[i]).IsDestroy)
                {
                    Monster c_Brick = (Monster)brick[i];   
                    if (c_Brick.BoundingBox.Intersects(NextPositionX()))
                    {
                        InverseHorizontalDirection();
                        c_Brick.RemoveLife(50);
                       
                    }
                    if (c_Brick.BoundingBox.Intersects(NextPositionY()))
                    {  
                        InverseVerticalDirection();
                        c_Brick.RemoveLife(50);

                      

                    }
                }
             
            }

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
                    Vitesse.X = -(float)Math.Cos(angle) * Speed;
                    Vitesse.Y = -(float)Math.Sin(angle) * Speed;


                }
                else if (relativePositionX < 2 * padDivideBySix)
                {

                    angle = MathHelper.ToRadians(35f);
                    Vitesse.X = -(float)Math.Cos(angle) * Speed;
                    Vitesse.Y = -(float)Math.Sin(angle) * Speed;

                }
                else if (relativePositionX < 3 * padDivideBySix)
                {

                    angle = MathHelper.ToRadians(75f);
                    Vitesse.X = (float)Math.Cos(angle) * Speed;
                    Vitesse.Y = -(float)Math.Sin(angle) * Speed;

                }
                else if (relativePositionX < 4 * padDivideBySix)
                {

                    angle = MathHelper.ToRadians(75f);
                    Vitesse.X = (float)Math.Cos(angle) * Speed;
                    Vitesse.Y = -(float)Math.Sin(angle) * Speed;

                }
                else if (relativePositionX < 5 * padDivideBySix)
                {

                    angle = MathHelper.ToRadians(35f);
                    Vitesse.X = (float)Math.Cos(angle) * Speed;
                    Vitesse.Y = -(float)Math.Sin(angle) * Speed;

                }
                else
                {

                    angle = MathHelper.ToRadians(20f);
                    Vitesse.X = (float)Math.Cos(angle) * Speed;
                    Vitesse.Y = -(float)Math.Sin(angle) * Speed;

                }


            }

            

            }

        public Rectangle NextPositionX()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point((int)Vitesse.X, 0));
            return nextPosition;
        }

        public Rectangle NextPositionY()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point(0, (int)Vitesse.Y));
            return nextPosition;
        }


        public override void Update(GameTime p_GameTime)
        {

        
            if (isFired)
            {
                CanMove = true;
                addTrailToBall(p_GameTime);
            }


            if (CollisionEvent)
            {
                timer -= (float)p_GameTime.ElapsedGameTime.TotalSeconds;
            }

            if (!CollisionEvent)
            {
                lastValidPosition = Position;
            }

          

            if (CanMove)
            {
                TryMove();
            }
            else
            {
                UpdatePositionIfFollowingSomething();
            }

            base.Update(p_GameTime);

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


            CollisionEvent = false;
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if ((!isFired) && (ServiceLocator.GetService<LevelManager>().currentState == LevelManager.LevelState.play))
            {
                DrawTrajectory(p_SpriteBatch);
            }



            base.Draw(p_SpriteBatch);


        }


        public void TryMove()
        {
            if (CollisionEvent)
            {
                Position = lastValidPosition;
                CollisionEvent = false;
            } 
            
            else
            {
                if (Position.X + currentTexture.Width > ServiceLocator.GetService<PlayerArea>().area.Right)
                {
                    Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.Right - BoundingBox.Width, Position.Y);
                    Debug.WriteLine("Ball sort à droite " + destination.X + " " + destination.Y);
                    InverseHorizontalDirection();
                }

                if (Position.X < ServiceLocator.GetService<PlayerArea>().area.X)
                {
                    Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.X, Position.Y);
                     Debug.WriteLine("Ball sort à gauche " + destination.X + " " + destination.Y);
                    InverseHorizontalDirection();
                }

                if (Position.Y < ServiceLocator.GetService<PlayerArea>().area.Y)
                {
                    Position = new Vector2(Position.X, ServiceLocator.GetService<PlayerArea>().area.Y);
                    Debug.WriteLine("Ball sort en haut " + destination.X + " " + destination.Y);
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

                //   Move(destination.X, destination.Y);
                Move(Vitesse);


            }




        }

        public void InverseHorizontalDirection()
        {
            //  destination.X *= -1;
            Vitesse = new Vector2(-Vitesse.X, Vitesse.Y);
        }

        public void InverseVerticalDirection()
        {
            // destination.Y *= -1;
            Vitesse = new Vector2(Vitesse.X, -Vitesse.Y);
        }

        public void Fire()
        {
            if (!isFired)
            {
                ObjectToFollow = null;
                CalcTrajectory();
                CanMove = true;
                isFired = true;
            }

        }



        public void addTrailToBall(GameTime p_GameTime)
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add((ServiceLocator.GetService<ContentManager>()).Load<Texture2D>("images/round"));


            TimedParticles particle = new TimedParticles(textures, (int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height, 1.3f);
            timedParticles.Add(particle);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(particle);
        }

        public void CalcTrajectory()
        {
            MouseState mouse = ServiceLocator.GetService<MouseState>();
            distanceFromMouse = Utils.calcDistance(Position.X, Position.Y, mouse.X, mouse.Y);
            angle = Utils.calcAngleWithMouse(Position.X, Position.Y);
            Vitesse = new Vector2(((float)Math.Cos(angle) * Speed), (float)Math.Sin(angle) * Speed);
      
        }

        public void DrawTrajectory(SpriteBatch p_SpriteBatch)
        {
            CalcTrajectory();
            Rectangle line = new Rectangle((int)Position.X + currentTexture.Width / 2, (int)Position.Y + currentTexture.Height / 2, (int)distanceFromMouse, 2);
            Rectangle reflectLine = new Rectangle((int)Position.X + currentTexture.Width / 2, (int)Position.Y + currentTexture.Height / 2, (int)distanceFromMouse, 2);
            p_SpriteBatch.Draw(lineTexture, line, line, Color.White, (float)angle, Vector2.Zero, SpriteEffects.None, 1);

        }

        public void Destroy(IDestroyable ball)
        {
            for (int n = timedParticles.Count() - 1; n >= 0; n--)
            {

                ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(timedParticles[n]);
                timedParticles.Remove(timedParticles[n]);

            }
            ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(this);

            IsDestroy = true;
            ball = null;
        }

        public void Destroy()
        {
            Destroy(this);
        }


        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }

        public void Move(Vector2 vitesse)
        {
            Position = new Vector2(Position.X + vitesse.X, Position.Y + vitesse.Y);
        }

    }

  

}