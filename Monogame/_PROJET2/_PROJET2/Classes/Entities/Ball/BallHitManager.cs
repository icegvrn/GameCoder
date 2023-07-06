using Microsoft.Xna.Framework;

namespace BricksGame
{
    /// <summary>
    /// Classe contenant l'indication d'un hit ou pas via un bool et un état de balle. Elle sert principalement à gérer le son du hit avec un timer.
    /// </summary>
    public class BallHitManager
    {
        private Ball ball;
        private bool hit;
        private float hitSoundDelay = 1f;
        private float hitTimer = 0f;
        private MediaPlayerService soundContainer;
        public BallHitManager(Ball p_ball)
        {
            ball = p_ball;
            soundContainer = new MediaPlayerService(ball);
        }

        // Update : permet de gérer un délais sur le son produit par le hit de la balle
        public void Update(GameTime p_GameTime)
        {
            if (hit)
            {
                hitTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
                if (hitTimer >= hitSoundDelay)
                {
                    ball.ChangeState(Gamesystem.BallState.fired);
                    hit = false;
                    hitTimer = 0;
                }
            }
        }

        // Méthode appelée par la ball quand la ball a hit quelque chose : déclanche la lecture du son hit
        public void HitEvents()
        {
            hit = true;
            ball.ChangeState(Gamesystem.BallState.hit);
            soundContainer.Play(Gamesystem.BallState.hit);
        }
    }
}
