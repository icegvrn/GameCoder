using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BricksGame
{
    /// <summary>
    /// Classe qui permet d'afficher la barre de vie du monstre
    /// </summary>
    public class MonsterHealthUI : HealthUI
        {
        private MonsterHealth monsterHealth;
      
        public MonsterHealthUI(MonsterHealth m_health)
        {
            monsterHealth = m_health;
            InitGauge();
        }

        public override void Update(GameTime p_gameTime)
        {
            gauge.Update(p_gameTime, (int)monsterHealth.Life, new Vector2(((int)(monsterHealth.Monster.Position.X - monsterHealth.Monster.MonsterWidth / 2)), (((int)monsterHealth.Monster.Position.Y + monsterHealth.Monster.MonsterHeight / 2) + 2)));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawLifeGauge(spriteBatch);
        }

        // Initialisation de la jauge de vie
        private void InitGauge()
        {
            lifeBarLenght = 56;
            lifeBarHeight = 4;
            Rectangle lifeBar = new Rectangle((int)monsterHealth.Monster.Position.X - monsterHealth.Monster.BoundingBox.Width / 2, (int)monsterHealth.Monster.Position.Y, lifeBarLenght, lifeBarHeight);
            gauge = new EvolutiveColoredGauge(monsterHealth.InitialLife, lifeBar, Color.White, threshold, lifeColors, true, new Vector2(monsterHealth.Monster.Position.X, monsterHealth.Monster.Position.Y), Color.White);
        }

        private void DrawLifeGauge(SpriteBatch p_SpriteBatch)
        {
            gauge.Draw(p_SpriteBatch);
        }
    }
}
