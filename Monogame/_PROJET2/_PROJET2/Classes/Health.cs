using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
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
