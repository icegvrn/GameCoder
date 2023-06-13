using BricksGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class Sprite : GameObject
    {

        public virtual bool CanMove { get; set; }
        public virtual Vector2 lastValidPosition { get; set; }
        public virtual float Speed { get; set; }

        protected virtual Sprite ObjectToFollow { get; set; }
        private Vector2 distanceWidthObjectToFollow { get; set; }

        //Sprite 
        public List<Texture2D> Textures { get; protected set; }
        public Texture2D currentTexture { get; protected set; }
  
        
        public Sprite(Texture2D p_texture)
        {
            currentTexture = p_texture;
            Speed = 1f;
        }

        public Sprite(List<Texture2D> p_texture)
        {
            Textures = p_texture;
            currentTexture = Textures[0];
            Speed = 1f;
        }

        public Sprite()
        {
        }

        public virtual void Move(float p_x, float p_y)
        {
                Position = new Vector2(Position.X + p_x * Speed, Position.Y + p_y * Speed);
        }

        public virtual void UpdatePositionIfFollowingSomething()
        {
            if (ObjectToFollow != null)
            {
                Position = ObjectToFollow.Position - distanceWidthObjectToFollow;
            }
        }

        public void Following(Sprite sprite)
        {
            ObjectToFollow = sprite;
            distanceWidthObjectToFollow = ObjectToFollow.Position - Position;
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            p_SpriteBatch.Draw(currentTexture, Position, Color.White);
        }

        public override void Update(GameTime p_GameTime)
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);
        }
    
    }
}
