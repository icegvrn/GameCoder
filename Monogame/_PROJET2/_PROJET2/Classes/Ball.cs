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


        public Ball(List<Texture2D> p_texture) : base(p_texture)
        {
            timedParticles = new List<TimedParticles>();
            lineTexture = (ServiceLocator.GetService<ContentManager>()).Load<Texture2D>("images/blank");
            Speed = 3f;
            CanMove = false;
    
        }


        public void TouchedBy(GameObject p_By)
        { 
            
            if (timer <= 0)
                {
                    CollisionEvent = false;
                    timer = 0.05f;
                }

            if (p_By is Monster)
            {

                Monster brick = (Monster)p_By;

                if (!CollisionEvent)
                {
                    if (BoundingBox.Intersects(brick.BoundingBox))
                    {
                      //  Debug.WriteLine("Collision détectée");

                        if (lastValidPosition.X >= brick.BoundingBox.X && lastValidPosition.X <= brick.BoundingBox.X + brick.BoundingBox.Width)
                        {
                        //    Debug.WriteLine("Inversion de direction verticale");
                            InverseVerticalDirection();
                        }
                        else
                        {
                         //   Debug.WriteLine("Inversion de direction horizontale");
                            InverseHorizontalDirection();
                        }
                        CollisionEvent = true;
                        brick.RemoveLife(2);
                    }
                }
            }

            else if (p_By is Player)
            {
                if (!CollisionEvent)
                {
                    Player player = (Player)p_By;

                    // Calcul de la position horizontale relative de la balle par rapport au pad
                    float relativePositionX = Position.X - player.Position.X;
                    float padWidth = player.BoundingBox.Width;
                    float quarterWidth = padWidth / 6f;
                  
                    // Déterminer la partie du pad où la balle a touché
                    if (relativePositionX < quarterWidth)
                    {
                        Debug.WriteLine("PREMIER QUART");
                        // Premier quart (tout à gauche)
                        angle = MathHelper.ToRadians(20f); // Angle de 60 degrés vers la gauche
                        destination.X = -(float)Math.Cos(angle) * Speed;
                        destination.Y = -(float)Math.Sin(angle) * Speed;


                    }
                    else if (relativePositionX < 2 * quarterWidth)
                    {
                        Debug.WriteLine("DEUXIEME QUART");
                        // Deuxième quart (gauche)
                        angle = MathHelper.ToRadians(30f); // Angle de 30 degrés vers la gauche
                        destination.X = -(float)Math.Cos(angle) * Speed;
                        destination.Y = -(float)Math.Sin(angle) * Speed;

                    }
                    else if (relativePositionX < 3 * quarterWidth)
                    {
                        Debug.WriteLine("TROISIEME QUART");
                        // Troisième quart (droite)
                        angle = MathHelper.ToRadians(70f); // Angle de 30 degrés vers la droite
                        destination.X = (float)Math.Cos(angle) * Speed;
                        destination.Y = -(float)Math.Sin(angle) * Speed;

                    }
                    else if (relativePositionX < 4 * quarterWidth)
                    {
                        Debug.WriteLine("TROISIEME QUART");
                        // Troisième quart (droite)
                        angle = MathHelper.ToRadians(70f); // Angle de 30 degrés vers la droite
                        destination.X = (float)Math.Cos(angle) * Speed;
                        destination.Y = -(float)Math.Sin(angle) * Speed;

                    }
                    else if (relativePositionX < 5 * quarterWidth)
                    {
                        Debug.WriteLine("TROISIEME QUART");
                        // Troisième quart (droite)
                        angle = MathHelper.ToRadians(30f); // Angle de 30 degrés vers la droite
                        destination.X = (float)Math.Cos(angle) * Speed;
                        destination.Y = -(float)Math.Sin(angle) * Speed;

                    }
                    else
                    {
                        Debug.WriteLine("QUATRIEME QUART");
                        // Quatrième quart (tout à droite)
                        angle = MathHelper.ToRadians(20f); // Angle de 60 degrés vers la droite
                        destination.X =(float)Math.Cos(angle) * Speed;
                        destination.Y = -(float)Math.Sin(angle) * Speed;

                    }
                    Debug.WriteLine("MA DESTINATION EST DE " + destination.X + " car j'avais un angle de " + angle);
                    CollisionEvent = true;

                }

            }


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

    

        public override void Update(GameTime p_GameTime)
        {
            if (isFired)
            {
                addTrailToBall(p_GameTime);
            }
         

            if (CollisionEvent)
            {
                timer -= (float)p_GameTime.ElapsedGameTime.TotalSeconds;
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


            for (int n=timedParticles.Count()-1; n>=0; n--)
            {
                if (timedParticles[n].timer <= 0)
                {
                    timedParticles.Remove(timedParticles[n]);
                    ServiceLocator.GetService<Scene>().RemoveToGameObjectsList(timedParticles[n]);
                }
                else
                {
                    timedParticles[n].timer -= (float)p_GameTime.ElapsedGameTime.TotalSeconds;
                }
            }

       

        }

        public void addTrailToBall(GameTime p_GameTime)
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add((ServiceLocator.GetService<ContentManager>()).Load<Texture2D>("images/round"));


            TimedParticles particle = new TimedParticles(textures, (int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height, 1.3f);
            timedParticles.Add(particle);
            ServiceLocator.GetService<Scene>().AddToGameObjectsList(particle);
        }

        public void CalcTrajectory()
        {
            MouseState mouse = ServiceLocator.GetService<MouseState>();
            distanceFromMouse = Utils.calcDistance(Position.X, Position.Y, mouse.X, mouse.Y);
            angle = Utils.calcAngleWithMouse(Position.X, Position.Y);
            destination = new Vector2(((float)Math.Cos(angle) * Speed), (float)Math.Sin(angle) * Speed);
            // Vérifier les collisions avec les bords de l'écran

     
        }

        public void DrawTrajectory(SpriteBatch p_SpriteBatch)
        {
            CalcTrajectory();
            Rectangle line = new Rectangle((int)Position.X+ currentTexture.Width/2, (int)Position.Y+currentTexture.Height/2, (int)distanceFromMouse, 2);
            Rectangle reflectLine = new Rectangle((int)Position.X + currentTexture.Width / 2, (int)Position.Y + currentTexture.Height / 2, (int)distanceFromMouse, 2);
            p_SpriteBatch.Draw(lineTexture, line, line, Color.White, (float)angle, Vector2.Zero, SpriteEffects.None, 1);
       
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

            if (Position.X + currentTexture.Width > ServiceLocator.GetService<PlayingAera>().aera.Right)
            {
                Position = new Vector2(ServiceLocator.GetService<PlayingAera>().aera.Right - currentTexture.Width, Position.Y);
                //    Debug.WriteLine("Ball sort à droite " + destination.X + " " + destination.Y);
                InverseHorizontalDirection();
            }

            if (Position.X < ServiceLocator.GetService<PlayingAera>().aera.X)
            {
                Position = new Vector2(ServiceLocator.GetService<PlayingAera>().aera.X, Position.Y);
             //   Debug.WriteLine("Ball sort à gauche " + destination.X + " " + destination.Y);
                InverseHorizontalDirection();
            }

             if (Position.Y < ServiceLocator.GetService<PlayingAera>().aera.Y)
            {
                Position = new Vector2(Position.X, ServiceLocator.GetService<PlayingAera>().aera.Y);
                // Debug.WriteLine("Ball sort en haut " + destination.X + " " + destination.Y);
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

         
            lastValidPosition = Position;
           
         
            Move(destination.X, destination.Y);
           
        }

        public void InverseHorizontalDirection()
        {
            destination.X *= -1;
        }

        public void InverseVerticalDirection()
        {
            destination.Y *= -1;
        }

        public void Destroy(IDestroyable ball)
        {
               for (int n = timedParticles.Count() - 1; n >= 0; n--)
            {
                
                    ServiceLocator.GetService<Scene>().RemoveToGameObjectsList(timedParticles[n]);
                timedParticles.Remove(timedParticles[n]);

            }
            ServiceLocator.GetService<Scene>().RemoveToGameObjectsList(this);

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
