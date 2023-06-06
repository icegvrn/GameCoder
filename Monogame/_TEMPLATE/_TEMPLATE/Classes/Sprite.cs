using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _TEMPLATE
{
    public class Sprite : IActor
    {
        public Vector2 Position { get; set; }
        public Rectangle BoundingBox { get; private set; }

        //Sprite 
        public List<Texture2D> Textures { get; }
        public Texture2D currentTexture { get; protected set; }
  


        public Sprite(List<Texture2D> p_texture)
        {
            Textures = p_texture;
            currentTexture = Textures[0];
        }

        public void Draw(SpriteBatch p_SpriteBatch)
        {
            p_SpriteBatch.Draw(currentTexture, Position, Color.White);
        }

        public virtual void Update(GameTime p_GameTime)
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);
        }

        public  void Move(float p_x, float p_y)
        {
            Position = new Vector2(Position.X + p_x, Position.Y + p_y);
        }

        public virtual void TouchedBy(IActor p_By)
        {

        }
    }
}
