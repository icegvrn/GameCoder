using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BricksGame
{
    /// <summary>
    /// Classe de particles, permet de différencier la balle de ses particules. Possède des informations de couleur et un timer.
    /// </summary>
    public class TimedParticles : Sprite, ICollider
    {
        public float timer { get; set; }
        public float duration { get; set; }
        public bool isCollide { private set; get; }
        public Color[] colors = new Color[] { Color.CornflowerBlue, Color.Red };
        public Color currentColor;

        public TimedParticles(List<Texture2D> p_texture, int x, int y, int width, int height, float duration) : base(p_texture)
        {
            timer = duration;
            BoundingBox = new Rectangle(x, y, width, height);
            Position = new Vector2(x, y);
            currentColor = colors[0];
        }

        public override void Update(GameTime gameTime)
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);

            if (timer <= 0)
            {
                ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(this);
            }
            else
            {
               timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
         p_SpriteBatch.Draw(currentTexture, Position, null, currentColor, 0, Vector2.Zero, new Vector2(timer/2, timer / 2), SpriteEffects.None, 1); ;

        }

        //Méthode propre aux gameObjects avec un collider
        public void TouchedBy(GameObject p_By)
        {
            // Non utilisée
        }
    }
}
