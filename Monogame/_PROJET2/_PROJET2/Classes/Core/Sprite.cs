using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;



namespace BricksGame
{
    /// <summary>
    /// Classe de base des sprites, hérite de GameObject
    /// </summary>
    public class Sprite : GameObject
    {
        //Texture(s) du sprite
        public List<Texture2D> Textures { get; protected set; }
        public Texture2D currentTexture { get; protected set; }

        // Etats du sprite
        public virtual bool CanMove { get; set; }

        // Caractéristiques du sprite
        public virtual float Speed { get; set; }
        public SpriteEffects spriteEffects { get; protected set; }
        public Vector2 origin { get; protected set; }
        public float rotation { get; protected set; }
        public Nullable<Rectangle> rectSource { get; protected set; }
        public Color color { get; protected set; }
        public Vector2 scale { get; protected set; }


        //Positionnement du sprite
        protected virtual Sprite ObjectToFollow { get; set; }
        private Vector2 distanceWidthObjectToFollow { get; set; }
        public virtual Vector2 lastValidPosition { get; set; }


        public Sprite(Texture2D p_texture)
        {
            currentTexture = p_texture;
            InitSprite();
        }

        public Sprite(List<Texture2D> p_texture)
        {
           Textures = p_texture;
           currentTexture = Textures[0];
           InitSprite();
        }


        public Sprite()
        {
        }

        //Update du sprite : on update les boundingBox
        public override void Update(GameTime p_GameTime)
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);
        }

        // Dessin du sprite
        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            p_SpriteBatch.Draw(currentTexture, Position, rectSource, color, rotation, origin, 1f, spriteEffects, 1f);
        }

        // Initialisation par défaut du sprite
        public void InitSprite()
        {
            Speed = 0f;
            origin = Vector2.Zero;
            rotation = 0;
            scale = Vector2.One;
            rectSource = null;
            color = Color.White;
            spriteEffects = SpriteEffects.None;
            CanMove = true;
        }

        // Méthode permettant de faire bouger le sprite selon sa vitesse
        public virtual void Move(float p_x, float p_y)
        {
            Position = new Vector2(Position.X + p_x * Speed, Position.Y + p_y * Speed);
        }

        // Méthode permettant de faire en sorte que le sprite suive un autre sprite (objectToFollow)
        public virtual void UpdatePositionIfFollowingSomething()
        {
            if (ObjectToFollow != null)
            {
                Position = ObjectToFollow.Position - distanceWidthObjectToFollow;
            }
        }

        // Méthode permettant de déterminer un autre sprite que ce sprite doit suivre
        public void Following(Sprite sprite)
        {
            ObjectToFollow = sprite;
            distanceWidthObjectToFollow = ObjectToFollow.Position - Position;
        }

    }
}