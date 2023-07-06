using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    /// <summary>
    /// PlayerPowerUI est la classe qui permet d'afficher la barre des points gagnés par le joueur. 
    /// </summary>
    public class PlayerPowerUI
    {
        private int barsLenght = 100;
        private int barsHeight = 12;
        private Texture2D pointsIcon;
        private ColoredGauge pointsBar;
        private PlayerPowerManager playerPowerManager;
        public PlayerPowerUI(PlayerPowerManager p_playerPowerManager)
        {
            playerPowerManager = p_playerPowerManager;

            // Initiation de la barre des points
            pointsIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetImagesRoot()+"icon_power");
            Rectangle rectPointsBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - 50 - barsLenght - pointsIcon.Width / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, barsLenght, barsHeight);
            pointsBar = new ColoredGauge(ServiceLocator.GetService<ISessionService>().GetMaxPoints(), rectPointsBar, Color.CornflowerBlue) ;

        }

        // Update de la valeur des points et update de la barre de points en conséquence
        public void Update(GameTime gameTime)
        {
            pointsBar.CurrentValue = ServiceLocator.GetService<ISessionService>().GetPoints();
            pointsBar.Update(gameTime);
        }

        // Dessin de la gauge de points et de l'icône associée
        public void Draw(SpriteBatch spriteBatch)
        {
            pointsBar.Draw(spriteBatch);
            spriteBatch.Draw(pointsIcon, new Vector2((pointsBar.Position.X + barsLenght) - pointsIcon.Width * 0.5f, pointsBar.Position.Y - pointsIcon.Height / 2), Color.White);

        }
    }
}
