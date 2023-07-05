

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    public abstract class HealthUI
    {
        protected EvolutiveColoredGauge gauge;
        protected int lifeBarLenght = 56;
        protected int lifeBarHeight = 4;
        protected Color[] lifeColors = { new Color(51, 225, 51), Color.Yellow, Color.Orange, Color.Red };
        protected float[] threshold = { 0.65f, 0.55f, 0.35f };
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
