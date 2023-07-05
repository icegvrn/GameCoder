using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
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
            pointsIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_power");
            Rectangle rectPointsBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - 50 - barsLenght - pointsIcon.Width / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, barsLenght, barsHeight);
            pointsBar = new ColoredGauge(ServiceLocator.GetService<ISessionService>().GetMaxPoints(), rectPointsBar, Color.CornflowerBlue) ;

        }

        public void Update(GameTime gameTime)
        {
            pointsBar.CurrentValue = ServiceLocator.GetService<ISessionService>().GetPoints();
            pointsBar.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pointsBar.Draw(spriteBatch);
            spriteBatch.Draw(pointsIcon, new Vector2((pointsBar.Position.X + barsLenght) - pointsIcon.Width * 0.5f, pointsBar.Position.Y - pointsIcon.Height / 2), Color.White);

        }
    }
}
