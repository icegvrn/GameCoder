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
        private double angle;
        private double angle2;
        private double distanceFromMouse;
        private Vector2 destination;
        private bool isFired;
        public bool IsDestroy { get; set; }
        private List<Rectangle> lines;


        public Ball(List<Texture2D> p_texture) : base(p_texture)
        {
            lineTexture = (ServiceLocator.GetService<ContentManager>()).Load<Texture2D>("images/blank");
            Speed = 3f;
            CanMove = false;
            lines = new List<Rectangle>();
        }



        public void TouchedBy(GameObject p_By)
        {
          
        }

        public void Fire()
        {
            ObjectToFollow = null;
            CalcTrajectory();
            CanMove = true;
            isFired = true;
        }

    

        public override void Update(GameTime p_GameTime)
        {

    

            if (CanMove)
            {
                TryMove();
            }
            else
            {
                UpdatePositionIfFollowingSomething();
            }

            base.Update(p_GameTime);
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
            if (!isFired)
            {
                DrawTrajectory(p_SpriteBatch);
            }
          
          base.Draw(p_SpriteBatch);
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

            else if (Position.Y >= ServiceLocator.GetService<GraphicsDevice>().Viewport.Height)
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
            
            IsDestroy = true;
        }

    }
}
