using Microsoft.Xna.Framework;

namespace BricksGame
{
    /// <summary>
    /// Cette classe gère les mouvements du joueur via une méthode move.
    /// </summary>
     public class PlayerMovement
    {
        private Player player;
         public PlayerMovement(Player p_player) 
        { 
            player = p_player;
        }

        // La méthode move vérifie que le joueur ne sorte pas de la zone de jeu avant de le déplacer. Si c'est le cas, elle le replace.
        public void Move(float p_x, float p_y)
        {
            if (player.Position.X + p_x * player.Speed < ServiceLocator.GetService<PlayerArea>().Area.X)
            {
                player.Position = new Vector2(ServiceLocator.GetService<PlayerArea>().Area.X, player.Position.Y);
            }

            else if ((player.Position.X + p_x * player.Speed) > ServiceLocator.GetService<PlayerArea>().Area.Right - player.Size.X)
            {
                player.Position = new Vector2(ServiceLocator.GetService<PlayerArea>().Area.Right - player.Size.X, player.Position.Y);
            }

            else
            {
                player.Move(p_x, p_y);
            }

        }
    }
}
