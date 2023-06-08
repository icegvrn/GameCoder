using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class Ball : Sprite, ICollider
    {
        private Texture2D lineTexture;
        private double angle;
        private double distanceFromMouse;
        private Vector2 destination;
        private bool isFired;

        public Ball(List<Texture2D> p_texture) : base(p_texture)
        {
            lineTexture = (ServiceLocator.GetService<ContentManager>()).Load<Texture2D>("images/blank");
            Speed = 2f;
            CanMove = false;
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
                Move(destination.X, destination.Y);
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
            destination = new Vector2(((float)Math.Cos(angle) * Speed), (float)Math.Sin(angle) * Speed);
        }

        public void DrawTrajectory(SpriteBatch p_SpriteBatch)
        {
            CalcTrajectory();
            Rectangle line = new Rectangle((int)Position.X+ currentTexture.Width/2, (int)Position.Y+currentTexture.Height/2, (int)distanceFromMouse, 2);
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

    }
}
