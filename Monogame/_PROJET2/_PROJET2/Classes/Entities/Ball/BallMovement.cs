using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BricksGame { 

    /// <summary>
    /// BallMovement est la classe qui gère les mouvements de la balle durant le jeu. Elle possède une vitesse, une direction qu'elle peut calculer etc.
    /// </summary>
    public class BallMovement
    {
        private Ball ball;
        public Vector2 SpeedVector;
        private double distanceFromMouse;

        // Direction de la balle
        private double angle;
        
        // Indicateur de direction
        private Texture2D lineTexture;
        public BallMovement(Ball p_ball, float p_speed)
        {
            ball = p_ball;
            ball.Speed = p_speed;
            SpeedVector = new Vector2(ball.Speed, -ball.Speed);
            lineTexture = ServiceLocator.GetService<IAssetsServices>().GetGameTexture(IAssetsServices.textures.blank);
            SetMovable(false);
        }

        // Méthode permettant de faire bouger la balle une fois qu'on a vérifié qu'elle était bien dans l'écran de jeu et éventuellement repositionné
        public void TryMove()
        {
            ClampScreenPosition();
            Move(SpeedVector);
        }

        // Méthode vérifiant que la balle reste bien dans l'écran de jeu : effectue un rebond si la balle sort de la zone de jeu (playerArea). 
        private void ClampScreenPosition()
        {
            if (ball.Position.X + ball.currentTexture.Width > ServiceLocator.GetService<PlayerArea>().Area.Right)
            {
                ball.Position = new Vector2(ServiceLocator.GetService<PlayerArea>().Area.Right - ball.BoundingBox.Width, ball.Position.Y);
                InverseHorizontalDirection();
            }

            if (ball.Position.X < ServiceLocator.GetService<PlayerArea>().Area.X)
            {
                ball.Position = new Vector2(ServiceLocator.GetService<PlayerArea>().Area.X, ball.Position.Y);
                InverseHorizontalDirection();
            }

            if (ball.Position.Y < ServiceLocator.GetService<PlayerArea>().Area.Y)
            {
                ball.Position = new Vector2(ball.Position.X, ServiceLocator.GetService<PlayerArea>().Area.Y);
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

        // Méthode permettant de faire bouger la balle à une certaine velocité
        private void Move(Vector2 velocity)
        {
            ball.Position = new Vector2(ball.Position.X + velocity.X, ball.Position.Y + velocity.Y);
        }

        // Méthode permettant d'inverser la direction de la balle en X
        public void InverseHorizontalDirection()
        {
            SpeedVector = new Vector2(-SpeedVector.X, SpeedVector.Y);
            ball.OnDirectionChange();
            
        }

        // Méthode permettant d'inverser la direction de la balle en Y
        public void InverseVerticalDirection()
        {
            SpeedVector = new Vector2(SpeedVector.X, -SpeedVector.Y);
            ball.OnDirectionChange();
        }

        // Méthode permettant de renvoyer la balle sur la gauche selon un angle donné
        public void Left(float p_angle)
        {
            angle = MathHelper.ToRadians(p_angle);
            SpeedVector.X = -(float)Math.Cos(angle) * ball.Speed;
            SpeedVector.Y = -(float)Math.Sin(angle) * ball.Speed;
        }

        // Méthode permettant de renvoyer la balle sur la droite selon un angle donné
        public void Right(float p_angle)
        {
            angle = MathHelper.ToRadians(p_angle);
            SpeedVector.X = (float)Math.Cos(angle) * ball.Speed;
            SpeedVector.Y = -(float)Math.Sin(angle) * ball.Speed;
        }


        // Méthode permettant de calculer le vecteur de velocité de la balle par rapport à un angle selon la position de la souris. Utilisé au moment de lancer la balle
        public void CalcTrajectory()
        {
            Vector2 mouse = ServiceLocator.GetService<IInputService>().GetMousePosition();
            distanceFromMouse = Utils.calcDistance(ball.Position.X, ball.Position.Y, mouse.X, mouse.Y);
            angle = Utils.calcAngleWithMouse(ball.Position.X, ball.Position.Y);
            SpeedVector = new Vector2(((float)Math.Cos(angle) * ball.Speed), (float)Math.Sin(angle) * ball.Speed);
        }

        // Méthode permettant de dessiner un indicateur de direction de la balle par rapport à la souris (utilisé au moment du lancement de la balle)
        public void DrawTrajectory(SpriteBatch p_SpriteBatch)
        {
            CalcTrajectory();
            Rectangle line = new Rectangle((int)ball.Position.X + ball.currentTexture.Width / 2, (int)ball.Position.Y + ball.currentTexture.Height / 2, (int)distanceFromMouse, 2); 
            p_SpriteBatch.Draw(lineTexture, line, line, Color.CornflowerBlue, (float)angle, Vector2.Zero, SpriteEffects.None, 1);

        }

        // Methode permettant de définir si la balle à le droit de bouger ou non.
        public void SetMovable(bool movable)
        {
            ball.CanMove = movable;
        }

        // Méthode permettant de modifier la vitesse actuelle de la balle en modifiant le vector de vélocité
        public void SetSpeed(float speed)
        {
            SpeedVector = new Vector2(speed, -speed);
        }


    }
}

