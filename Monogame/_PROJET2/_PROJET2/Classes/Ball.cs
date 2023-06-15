using BricksGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class Ball : Sprite, ICollider, IDestroyable
    {
        private Texture2D lineTexture;
        public List<TimedParticles> timedParticles { get; set; }
        private double angle;
        private double angle2;
        private double distanceFromMouse;
        private Vector2 destination;
        private bool isFired;
        public bool collide = false;
        public bool IsDestroy { get; set; }
        private List<Rectangle> lines;
        public bool CollisionEvent = false;
        private float timer = 0.2f;


        public Ball(List<Texture2D> p_texture) : base(p_texture)
        {
            timedParticles = new List<TimedParticles>();
            lineTexture = (ServiceLocator.GetService<ContentManager>()).Load<Texture2D>("images/blank");
            Speed = 3f;
            CanMove = false;
            lines = new List<Rectangle>();
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
                        Debug.WriteLine("Collision détectée");

                        if (lastValidPosition.X >= brick.BoundingBox.X && lastValidPosition.X <= brick.BoundingBox.X + brick.BoundingBox.Width)
                        {
                            Debug.WriteLine("Inversion de direction verticale");
                            InverseVerticalDirection();
                        }
                        else
                        {
                            Debug.WriteLine("Inversion de direction horizontale");
                            InverseHorizontalDirection();
                        }
                        CollisionEvent = true;
                        brick.RemoveLife(1);


                    }
                }
            }

            else if (p_By is Pad)
            {
                if (!CollisionEvent)
                {
                    Pad brick = (Pad)p_By;
                    InverseVerticalDirection();
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
            textures.Add(currentTexture);
            TimedParticles particle = new TimedParticles(textures, (int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height, 0.6f) ;
            timedParticles.Add(particle);
            ServiceLocator.GetService<Scene>().AddToGameObjectsList(particle);
        }

        public void CalcTrajectory()
        {
            MouseState mouse = ServiceLocator.GetService<MouseState>();
            distanceFromMouse = Utils.calcDistance(Position.X, Position.Y, mouse.X, mouse.Y);
            angle = Utils.calcAngleWithMouse(Position.X, Position.Y);
            angle2 = Utils.calcAngleWithMouse(-Position.X, Position.Y);
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

            foreach (TimedParticles particle in timedParticles)
            {
                p_SpriteBatch.Draw(currentTexture, new Vector2(particle.BoundingBox.X, particle.BoundingBox.Y), Color.White);
            }
        }

        public void TryMove()
        {

            if (Position.X + currentTexture.Width >= ServiceLocator.GetService<GraphicsDevice>().Viewport.Width)
            {
                Debug.WriteLine("Ball sort à droite " + destination.X + " " + destination.Y);
                InverseHorizontalDirection();
            }

            else if (Position.X <= 0)
            {
                Debug.WriteLine("Ball sort à gauche " + destination.X + " " + destination.Y);
                InverseHorizontalDirection();
            }

            else if (Position.Y <= 0)
            {
                Debug.WriteLine("Ball sort en haut " + destination.X + " " + destination.Y);
                InverseVerticalDirection();
            }

            else if (Position.Y >= ServiceLocator.GetService<GraphicsDevice>().Viewport.Height *1.5f)
            {
                // Détruire la ball ici
                Destroy();

            }

            else
            {
            lastValidPosition = Position;
            }
         
            Move(destination.X, destination.Y);
           
        }

        public void InverseHorizontalDirection()
        {
            Debug.WriteLine("J'INVERSE");
            destination.X *= -1;
        }

        public void InverseVerticalDirection()
        {
            Debug.WriteLine("J'INVERSE");
            destination.Y *= -1;
        }

        public void Destroy()
        {
               for (int n = timedParticles.Count() - 1; n >= 0; n--)
            {
                
                    ServiceLocator.GetService<Scene>().RemoveToGameObjectsList(timedParticles[n]);
                timedParticles.Remove(timedParticles[n]);

            }
            IsDestroy = true;


         

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
