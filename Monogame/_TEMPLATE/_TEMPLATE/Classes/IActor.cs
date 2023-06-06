using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _TEMPLATE
{
    public interface IActor
    {
        Vector2 Position { get; }
        Rectangle BoundingBox { get; }
        void Update(GameTime p_GameTime);
        void Draw(SpriteBatch p_SpriteBatch);
        void TouchedBy(IActor p_By);
    }
}
