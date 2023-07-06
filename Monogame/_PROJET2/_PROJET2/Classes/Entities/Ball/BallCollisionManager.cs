using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BricksGame
{
    /// <summary>
    /// Classe gérant les collisions de la balle avec les autres acteurs du jeu
    /// </summary>
    public class BallCollisionManager
    {
        private Ball ball;
        private BallMovement ballMovement;
        public BallCollisionManager(Ball p_ball, BallMovement p_ballMovement)
        {
            ball = p_ball;
            ballMovement = p_ballMovement;
        }

        // Méthode appelée pour vérifier les collisions avec une liste de brickable object (ici les monstres). Compare la boundingBox des bricks avec les nextPosition de la balle, puis appelle de l'event collision et inversion de la balle 
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

        // Méthode appelée pour vérifier les collisions un joueur (joue le rôle du pad). Compare la boundingBox du joueur avec les nextPosition de la balle,
        // puis appelle des méthodes de ballMovement pour rediriger la balle selon un angle précis. Le pad est divisé en 6 pour changer l'angle de renvoi et amener des possibiltiés de gameplay au joueur.
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

        // Méthode déterminant la NextPosition en X de la balle (ou plutôt de sa BoundingBox en l'occurence)
        public Rectangle NextPositionX()
        {
            Rectangle nextPosition = ball.BoundingBox;
            nextPosition.Offset(new Point((int)ballMovement.SpeedVector.X, 0));
            return nextPosition;
        }

        // Méthode déterminant la NextPosition en Y de la balle (ou plutôt de sa BoundingBox en l'occurence)
        public Rectangle NextPositionY()
        {
            Rectangle nextPosition = ball.BoundingBox;
            nextPosition.Offset(new Point(0, (int)ballMovement.SpeedVector.Y));
            return nextPosition;
        }

         // Utilisé pour le debug : permet de Draw la boundingBox au besoin
        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = ball.BoundingBox;
            Texture2D blankTexture = ServiceLocator.GetService<IAssetsServices>().GetGameTexture(IAssetsServices.textures.blank);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }
    }
}
