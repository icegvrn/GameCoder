using Microsoft.Xna.Framework;

namespace BricksGame
{
    public abstract class Fighter
    {
        public bool IsAttacker { get; protected set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Attack();
        public abstract void StartAttack();
    }
}
