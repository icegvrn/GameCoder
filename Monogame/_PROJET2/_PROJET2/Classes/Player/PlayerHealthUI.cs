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
    public class PlayerHealthUI
    {
      
        private PlayerHealth playerHealth;
        // Barres du joueur
        private int barsLenght = 100;
        private int barsHeight = 12;
        // Barre de vie
        private EvolutiveColoredGauge barLife;
        private Texture2D lifeIcon;

        private Color[] lifeColors = { Color.Green, Color.Yellow, Color.Orange, Color.Red };
        float[] threshold = { 0.65f, 0.55f, 0.35f };

        public PlayerHealthUI(PlayerHealth p_health) 
        {
            playerHealth = p_health;
            lifeIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_heart");
            Rectangle lifeBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 + 50, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, barsLenght, barsHeight);
            barLife = new EvolutiveColoredGauge(playerHealth.InitialLife, lifeBar, Color.White, threshold, lifeColors);

        }

        public void Update(GameTime p_gameTime)
        {
         

            barLife.Update(p_gameTime, PlayerState.Life, barLife.Position);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            barLife.Draw(spriteBatch);
            spriteBatch.Draw(lifeIcon, new Vector2((barLife.Position.X + barsLenght) - lifeIcon.Width * 0.5f, barLife.Position.Y - lifeIcon.Height / 3), Color.White);

        }
    }
}
