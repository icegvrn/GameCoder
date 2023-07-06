using Microsoft.Xna.Framework;

namespace BricksGame
{
    /// <summary>
    /// Classe abstraite représentant l'élément combattant d'un objet
    /// </summary>
    public abstract class Fighter
    {
        public bool IsAttacker { get; protected set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Attack();
        public abstract void StartAttack();
    }
}
