using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BricksGame
{
    /// <summary>
    /// Classe gérant les collisions du joueur. Mériterait d'être revue car peu utilisée.Contient l'information de si le player est Hit ou pas. (Convertir en hitManager ?)
    /// </summary>
    public class PlayerCollisionsManager
    {
        private Player player;
        public bool IsPlayerHit { get; set; }
        public PlayerCollisionsManager(Player p_player) 
        {
        }

        // Méthode appelé par joueur quand le joueur est hit
        public void OnHit(bool b_hit)
        {
            IsPlayerHit = b_hit;
        }

        public Rectangle NextPositionY()
        {
            Rectangle nextPosition = player.BoundingBox;
            nextPosition.Offset(new Point(0, (int)(player.Speed)));
            return nextPosition;
        }

        public Rectangle NextPositionX()
        {
            Rectangle nextPosition = player.BoundingBox;
            nextPosition.Offset(new Point((int)(player.Speed), 0));
            return nextPosition;
        }

        // Pour le debug : permet d'afficher la boundingBox du joueur
        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = player.BoundingBox;
            Texture2D boxTexture = ServiceLocator.GetService<IAssetsServices>().GetGameTexture(IAssetsServices.textures.blank);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }
    }
}
