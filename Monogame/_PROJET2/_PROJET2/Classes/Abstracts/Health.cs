using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace BricksGame
{
    /// <summary>
    /// Classe abstraite représentant la vie d'un objet
    /// </summary>
    public abstract class Health
    {
        public float Life { get; protected set; }
        public float InitialLife { get; protected set; }
        public float ProvisoryLife { get; protected set; }
        public bool IsDead { get; protected set; }

        public abstract void InitLife(float nb);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Damage(float damage);
    }
}
