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

        public SpriteEffects spriteEffects { get; protected set; }
        public Vector2 origin { get; protected set; }

        public float rotation { get; protected set; }

        public Nullable<Rectangle> rectSource { get; protected set; }

        public Color color { get; protected set; }

        public Vector2 scale { get; protected set; }


        public Sprite(Texture2D p_texture)
        {
            Speed = 0f;
            currentTexture = p_texture;
            origin = Vector2.Zero;
            rotation = 0;
            scale = Vector2.One;
            rectSource = null;
            color = Color.White;
            spriteEffects = SpriteEffects.None;
            CanMove = true;
        }

        public Sprite(List<Texture2D> p_texture)
        {

            Textures = p_texture;
            currentTexture = Textures[0];
            Speed = 0f;
            origin = Vector2.Zero;
            rotation = 0;
            scale = Vector2.One;
            rectSource = null;
            color = Color.White;
            spriteEffects = SpriteEffects.None;
            CanMove = true;
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
            p_SpriteBatch.Draw(currentTexture, Position, rectSource, color, rotation, origin, 1f, spriteEffects, 1f);
        }

        public override void Update(GameTime p_GameTime)
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);
        }

    }
}