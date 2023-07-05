using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace BricksGame { 
    public class BallCollisionManager
    {
        private Ball ball;
        private BallMovement ballMovement;
        public BallCollisionManager(Ball p_ball, BallMovement p_ballMovement) 
        { 
        ball = p_ball;
        ballMovement = p_ballMovement;
 
        }

        public void Update(GameTime p_gameTime)
        {

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
                        ballMovement.InverseVerticalDirection();
                        ball.OnCollide(c_Brick);
                    }

                    if (c_Brick.BoundingBox.Intersects(NextPositionX()))
                    {
                        ballMovement.InverseHorizontalDirection();
                        ball.OnCollide(c_Brick);
                    }
                }
            }

        }

        public void CheckCollision(Player player)
        {
            float relativePositionX = ball.Position.X - player.Position.X;
            float padDivideBySix = player.BoundingBox.Width / 6f;

            if (player.BoundingBox.Intersects(NextPositionX()) || player.BoundingBox.Intersects(NextPositionY()))
            {
                if (relativePositionX < padDivideBySix)
                {
                    ballMovement.Left(20f);
                }
                else if (relativePositionX < 2 * padDivideBySix)
                {
                    ballMovement.Left(35f);
                }

                else if (relativePositionX < 3 * padDivideBySix)
                {
                    ballMovement.Left(75f);
                }

                else if (relativePositionX < 4 * padDivideBySix)
                {
                    ballMovement.Right(75f);
                }

                else if (relativePositionX < 5 * padDivideBySix)
                {
                    ballMovement.Right(35f);
                }

                else
                {
                    ballMovement.Right(20f);
                }
                ball.OnCollide(player);
               
            }
        }

        public Rectangle NextPositionX()
        {
            Rectangle nextPosition = ball.BoundingBox;
            nextPosition.Offset(new Point((int)ballMovement.SpeedVector.X, 0));
            return nextPosition;
        }

        public Rectangle NextPositionY()
        {
            Rectangle nextPosition = ball.BoundingBox;
            nextPosition.Offset(new Point(0, (int)ballMovement.SpeedVector.Y));
            return nextPosition;
        }
    }
}
