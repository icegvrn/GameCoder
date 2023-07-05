using Microsoft.Xna.Framework;

namespace BricksGame
{
     public class PlayerMouvement
    {
        private Player player;
         public PlayerMouvement(Player p_player) 
        { 
            player = p_player;
        }

        public void Move(float p_x, float p_y)
        {
            if (player.Position.X + p_x * player.Speed < ServiceLocator.GetService<PlayerArea>().area.X)
            {
                player.Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.X, player.Position.Y);
            }

            else if ((player.Position.X + p_x * player.Speed) > ServiceLocator.GetService<PlayerArea>().area.Right - player.Size.X)
            {
                player.Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.Right - player.Size.X, player.Position.Y);
            }

            else
            {
                player.Move(p_x, p_y);
            }

        }
    }
}
