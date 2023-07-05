using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace BricksGame { 
    public class BallMovement
    {
        private Ball ball;
        // Direction de la balle
        private double angle;
        public Vector2 SpeedVector;
        private double distanceFromMouse;
        private Texture2D lineTexture;
        public BallMovement(Ball p_ball, float p_speed)
        {
            ball = p_ball;
            ball.Speed = p_speed;
            SpeedVector = new Vector2(ball.Speed, -ball.Speed);
            lineTexture = ServiceLocator.GetService<IAssetsServices>().GetGameTexture(IAssetsServices.textures.blank);
            SetMovable(false);
        }

        public void TryMove()
        {
            ClampScreenPosition();
            Move(SpeedVector);
        }

        private void Move(Vector2 velocity)
        {
            ball.Position = new Vector2(ball.Position.X + velocity.X, ball.Position.Y + velocity.Y);
        }

        private void ClampScreenPosition()
        {
            if (ball.Position.X + ball.currentTexture.Width > ServiceLocator.GetService<PlayerArea>().area.Right)
            {
                ball.Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.Right - ball.BoundingBox.Width, ball.Position.Y);
                InverseHorizontalDirection();
            }

            if (ball.Position.X < ServiceLocator.GetService<PlayerArea>().area.X)
            {
                ball.Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.X, ball.Position.Y);
                InverseHorizontalDirection();
            }

            if (ball.Position.Y < ServiceLocator.GetService<PlayerArea>().area.Y)
            {
                ball.Position = new Vector2(ball.Position.X, ServiceLocator.GetService<PlayerArea>().area.Y);
                InverseVerticalDirection();
            }

            if (ball.Position.Y > ServiceLocator.GetService<GraphicsDevice>().Viewport.Height * 1.5f)
            {
                ball.OutOfScreen();
               
            }

            if (ball.Position.Y < 0)
            {
                ball.OutOfScreen();
            }
        }


        public void SetSpeed(float speed)
        {
            SpeedVector = new Vector2(speed, -speed);
        }

        public void CalcTrajectory()
        {
            Vector2 mouse = ServiceLocator.GetService<IInputService>().GetMousePosition();
            distanceFromMouse = Utils.calcDistance(ball.Position.X, ball.Position.Y, mouse.X, mouse.Y);
            angle = Utils.calcAngleWithMouse(ball.Position.X, ball.Position.Y);
            SpeedVector = new Vector2(((float)Math.Cos(angle) * ball.Speed), (float)Math.Sin(angle) * ball.Speed);
        }

        public void InverseHorizontalDirection()
        {
            SpeedVector = new Vector2(-SpeedVector.X, SpeedVector.Y);
            ball.OnDirectionChange();
            
        }

        public void InverseVerticalDirection()
        {
            SpeedVector = new Vector2(SpeedVector.X, -SpeedVector.Y);
            ball.OnDirectionChange();
        }

        public void Left(float p_angle)
        {
            angle = MathHelper.ToRadians(p_angle);
            SpeedVector.X = -(float)Math.Cos(angle) * ball.Speed;
            SpeedVector.Y = -(float)Math.Sin(angle) * ball.Speed;
        }

        public void Right(float p_angle)
        {
            angle = MathHelper.ToRadians(p_angle);
            SpeedVector.X = (float)Math.Cos(angle) * ball.Speed;
            SpeedVector.Y = -(float)Math.Sin(angle) * ball.Speed;
        }

        public void DrawTrajectory(SpriteBatch p_SpriteBatch)
        {
            CalcTrajectory();
            Rectangle line = new Rectangle((int)ball.Position.X + ball.currentTexture.Width / 2, (int)ball.Position.Y + ball.currentTexture.Height / 2, (int)distanceFromMouse, 2);
            Rectangle reflectLine = new Rectangle((int)ball.Position.X + ball.currentTexture.Width / 2, (int)ball.Position.Y + ball.currentTexture.Height / 2, (int)distanceFromMouse, 2);
            p_SpriteBatch.Draw(lineTexture, line, line, Color.CornflowerBlue, (float)angle, Vector2.Zero, SpriteEffects.None, 1);

        }

        public void SetMovable(bool movable)
        {
            ball.CanMove = movable;
        }

    }
}

