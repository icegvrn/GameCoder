using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class BallParticlesManager
    {
        Ball ball;
        public List<TimedParticles> timedParticles { get; set; }
        public BallParticlesManager(Ball p_ball) 
        { 
            ball = p_ball;
            timedParticles = new List<TimedParticles>();
        }

        public void AddParticles(GameTime p_GameTime)
        {
            List<Texture2D> textures = new List<Texture2D> { (ServiceLocator.GetService<ContentManager>()).Load<Texture2D>("images/round") };
            TimedParticles particle = new TimedParticles(textures, (int)ball.Position.X, (int)ball.Position.Y, ball.currentTexture.Width, ball.currentTexture.Height, 1.3f);
            timedParticles.Add(particle);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(particle);
        }

        public void Update(GameTime p_GameTime)
        {
            UpdateParticles(p_GameTime);
        }


        private void UpdateParticles(GameTime p_GameTime)
        {
            for (int n = timedParticles.Count() - 1; n >= 0; n--)
            {
                if (timedParticles[n].timer <= 0)
                {
                    timedParticles.Remove(timedParticles[n]);
                    ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(timedParticles[n]);
                }
                else
                {
                    timedParticles[n].timer -= (float)p_GameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public void DestroyAll()
        {
            for (int n = timedParticles.Count() - 1; n >= 0; n--)
            {
                ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(timedParticles[n]);
                timedParticles.Remove(timedParticles[n]);
            }
        }

        public void PoweredColor(bool enable)
        {
                foreach (TimedParticles particle in timedParticles)
            {
                if (enable)
                {
                    particle.currentColor = particle.colors[1];
                } 
                else
                {
                    particle.currentColor = particle.colors[0];
                }
                } 
        }
    }
}
