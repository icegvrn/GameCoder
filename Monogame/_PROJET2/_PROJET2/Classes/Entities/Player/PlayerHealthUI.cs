using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    /// <summary>
    /// Classe qui permet d'afficher la barre de vie du joueur
    /// </summary>
    public class PlayerHealthUI : HealthUI
    {
        private PlayerHealth playerHealth;
        private Texture2D lifeIcon;

        public PlayerHealthUI(PlayerHealth p_health)
        {
            playerHealth = p_health;
            InitGauge();
        }

        public override void Update(GameTime p_gameTime)
        {
            gauge.Update(p_gameTime, ServiceLocator.GetService<ISessionService>().GetLife(), gauge.Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gauge.Draw(spriteBatch);
            spriteBatch.Draw(lifeIcon, new Vector2((gauge.Position.X + lifeBarLenght) - lifeIcon.Width * 0.5f, gauge.Position.Y - lifeIcon.Height / 3), Color.White);
        }

        //Initialisation de le barre de vie
        public void InitGauge()
        {
            lifeBarLenght = 100;
            lifeBarHeight = 12;
            lifeIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_heart");
            Rectangle lifeBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 + 50, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, lifeBarLenght, lifeBarHeight);
            gauge = new EvolutiveColoredGauge(playerHealth.InitialLife, lifeBar, Color.White, threshold, lifeColors);
        }
    }
}
