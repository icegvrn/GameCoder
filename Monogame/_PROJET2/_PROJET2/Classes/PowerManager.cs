using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    public abstract class PowerManager
    {
        protected Power Power;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void ActivatePower();
        public abstract void TriggerPower();
        public abstract void ResetPower();
    }
}