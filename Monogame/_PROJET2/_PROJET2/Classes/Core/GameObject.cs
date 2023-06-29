using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public abstract class GameObject
    {
        private Vector2 position { get; set; }
        public Vector2 Position { get { return position; } set { position = value; } }
        private Rectangle boundingBox { get; set; }
        public Rectangle BoundingBox { get { return boundingBox; }  set { boundingBox = value; }}

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch p_SpriteBatch);

       
    }
}
