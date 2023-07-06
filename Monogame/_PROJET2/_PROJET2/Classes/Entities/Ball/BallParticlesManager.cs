using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BricksGame
{
    /// <summary>
    /// Classe qui gère les particules de la ball en instanciant des TimedParticules et en les gardant dans une liste. 
    /// </summary>
    public class BallParticlesManager
    {
        private Ball ball;
        private List<TimedParticles> timedParticles;
        public BallParticlesManager(Ball p_ball) 
        { 
            ball = p_ball;
            timedParticles = new List<TimedParticles>();
        }

        // Méthode créant et ajoutant une nouvelle TimedParticule à la liste. Définition de l'image utilisé, du temps de vie de la particule et ajout de la particule dans les objets de la scène
        public void AddParticles(GameTime p_GameTime)
        {
            List<Texture2D> textures = new List<Texture2D> { (ServiceLocator.GetService<ContentManager>()).Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetImagesRoot()+"round") };
            TimedParticles particle = new TimedParticles(textures, (int)ball.Position.X, (int)ball.Position.Y, ball.currentTexture.Width, ball.currentTexture.Height, 1.3f);
            timedParticles.Add(particle);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(particle);
        }

        public void Update(GameTime p_GameTime)
        {
            RemoveDeadParticles(p_GameTime);
        }

        // Méthode permettant de changer la couleur des particules pour les passer en mode "chargée de pouvoir" ou non
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


        //Méthode permettant d'enlever de la list des particules les particules qui sont arrivées en fin de vie.
        private void RemoveDeadParticles(GameTime p_GameTime)
        {
            for (int n = timedParticles.Count() - 1; n >= 0; n--)
            {
                timedParticles[n].Update(p_GameTime);

                if (timedParticles[n].timer <= 0)
                {
                    timedParticles.Remove(timedParticles[n]);
                }
            }
        }


        // Destruction de toutes les particules en cours (utilisée notamment au changement de niveau)
        public void DestroyAll()
        {
            for (int n = timedParticles.Count() - 1; n >= 0; n--)
            {
                ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(timedParticles[n]);
                timedParticles.Remove(timedParticles[n]);
            }
        }

    }
}
