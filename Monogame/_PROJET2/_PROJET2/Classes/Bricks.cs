using BricksGame;
using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;

namespace BricksGame
{
    public class Bricks : Sprite, ICollider, IDestroyable, IBrickable
    {
        private bool isDestroy;
        public bool IsDestroy { get { return isDestroy; } set { isDestroy = value; } }
        public bool CollisionEvent = false;
        private int life = 4;
        private float timer = 0.2f;
        public Bricks(List<Texture2D> p_textures): base(p_textures) 
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);

        }

        public Bricks(List<Texture2D> p_textures, int Power) : base(p_textures)
        {
            life = Power;
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);

        }
        public void TouchedBy(GameObject p_By)
        { 
              
            
            if (p_By is Ball)
                {
                Ball ball = (Ball)p_By;
              

                if (timer <= 0)
                {
                    CollisionEvent = false;
                    timer = 0.05f;
                }

                if (!CollisionEvent)
                  {
                  
                    Debug.WriteLine(Position.Y + ":" + currentTexture.Height + " et en total " + (Position.Y + currentTexture.Height));
                    Debug.WriteLine(Position.X + ":" + currentTexture.Width + " et en total " + (Position.X + currentTexture.Width));
                    Debug.WriteLine("Ball position X et Y " + ball.Position.X + ":" + ball.Position.Y + " et brick : " + (Position.X + currentTexture.Width) + "," + (Position.Y + currentTexture.Height));
                    Debug.WriteLine("La dernière position de la balle était :" + ball.lastValidPosition.X + ":"+ ball.lastValidPosition.Y);

                    if ((ball.Position.Y + ball.currentTexture.Height >= Position.Y) && (ball.Position.Y <= Position.Y + currentTexture.Height) && (ball.Position.X +ball.currentTexture.Width >= Position.X) && (ball.Position.X <= Position.X + currentTexture.Width))
                    {
                        Debug.WriteLine("J'inverse la direction X");
                     

                        if ((ball.lastValidPosition.X >= Position.X) && (ball.lastValidPosition.X <= Position.X + currentTexture.Width))
                        {

                            Debug.WriteLine("J'inverse la direction Y");
                            ball.InverseVerticalDirection();
                           
                        }

                        else
                        {
                            ball.InverseHorizontalDirection();
                        }
                        ball.Position = ball.lastValidPosition;
                        RemoveLife();
                        
                        CollisionEvent = true;
                    }
                   
                }
            }
           
        }

      

        public override void Update(GameTime p_GameTime)
        {
            if  (life <= 0)
            {
                Destroy();
            }

            if (CollisionEvent)
            {
                timer -= (float)p_GameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(p_GameTime);
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {

            p_SpriteBatch.Draw(currentTexture, new Vector2(Position.X - currentTexture.Width/2, Position.Y-currentTexture.Height/2), Color.White);

            p_SpriteBatch.DrawString(AssetsManager.MainFont, life.ToString(), Position, Color.Black);
        }

    
        public void Destroy()
        {
            isDestroy = true;
        }

        public void RemoveLife()
        {
            life--;
            CollisionEvent = false;
        }
    }
}

