using System.Drawing;

namespace BricksGame
{
    /// <summary>
    /// Classe permettant de déterminer une limite de jeu sur la map : utilisé pour les collisions notamment
    /// </summary>
    public class PlayerArea 
    {
        public Rectangle Area { get; private set; }

        public PlayerArea(int x , int y, int width, int height)
        {
            Area = new Rectangle(x, y, width, height);
        }
    }
}
