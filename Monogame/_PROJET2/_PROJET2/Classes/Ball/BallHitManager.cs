using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
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

        public void HitEvents()
        {
            hit = true;
            ball.ChangeState(Gamesystem.BallState.hit);
            soundContainer.Play(Gamesystem.BallState.hit);
        }
    }
}
