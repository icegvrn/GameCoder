using Microsoft.Xna.Framework;


namespace BricksGame
{
    /// <summary>
    /// Classe permettant de gérer les input utlisateurs qui vont contrôler le joueur.
    /// </summary>
    public class PlayerInput
    {
        private Player player;
        public bool CanMove;
        public PlayerInput(Player p_player)
        {
         player = p_player;
         CanMove = true;
        }

        public void Update(GameTime gameTime)
        {
            if (player.IsReady && CanMove)
            {

            // Gestion des mouvements du joueur
            if (ServiceLocator.GetService<IInputService>().OnLeftDown())
            {
                player.Left();
            }
            else if (ServiceLocator.GetService<IInputService>().OnRightDown())
            {
                player.Right();
            }
            else
            {
                player.Stay();
            }

            if (ServiceLocator.GetService<IInputService>().OnLeftReleased())
            {
                player.Stay();
            }
            else if (ServiceLocator.GetService<IInputService>().OnRightReleased())
            {
                player.Stay();
            }

            // Gestion des actions du joueur
            if (ServiceLocator.GetService<IInputService>().OnActionReleased())
            {
                player.Action();
            }
        }
        }
    }
}

