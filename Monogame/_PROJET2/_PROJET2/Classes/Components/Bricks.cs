
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace BricksGame
{
    /// <summary>
    /// Classe de base "brique" qui contient les interfaces et une version "par défaut" d'une brique : une texture, une vie, une boundingBox
    /// </summary>
    public class Bricks : Sprite, ICollider, IDestroyable, IBrickable
    {
        // Etats de la brique
        public bool IsDestroy { get; set; }

        // Numéro de slot sur la grille
        public float GridSlotNb { get; set; }

        // Caractéristiques
        private int life;


        public Bricks(Texture2D p_texture) : base(p_texture) 
        {
            // Initialisation de la boundingBox
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);

        }

        public Bricks(Texture2D p_texture, int Power) : base(p_texture)
        {
            // Initialisation de la boundingBox et de la vie s'il y a une vie
            life = Power;
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);
        }
    

        // Destruction de la brique si sa vie est à 0
        public override void Update(GameTime p_GameTime)
        {
            if  (life <= 0)
            {
                Destroy(this);
            }

            base.Update(p_GameTime);
        }

        // Draw de la brique et de sa vie
        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            p_SpriteBatch.Draw(currentTexture, new Vector2(Position.X - currentTexture.Width/2, Position.Y-currentTexture.Height/2), Color.White);

            p_SpriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.MainFont), life.ToString(), Position, Color.Black);
           
        }

        public void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            Texture2D pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);

            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }

        // Méthodes de destruction de la brique
        public void Destroy()
        {
            Destroy(this);
        }
        public void Destroy(IDestroyable brick)
        {
            IsDestroy = true;
            brick = null;
        }

        // Méthodes propres aux gameObject
        public void TouchedBy(GameObject p_By)
        {
        }

    }
}

