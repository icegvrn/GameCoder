using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BricksGame
{
        public class MonsterHealthUI : HealthUI
        {
        private MonsterHealth monsterHealth;
      
        public MonsterHealthUI(MonsterHealth m_health)
        {
            lifeBarLenght = 56;
            lifeBarHeight = 4;
            monsterHealth = m_health;
            AddGauge();
        }

        private void AddGauge()
        {
            Rectangle lifeBar = new Rectangle((int)monsterHealth.Monster.Position.X - monsterHealth.Monster.BoundingBox.Width / 2, (int)monsterHealth.Monster.Position.Y, lifeBarLenght, lifeBarHeight);
            gauge = new EvolutiveColoredGauge(monsterHealth.InitialLife, lifeBar, Color.White, threshold, lifeColors, true, new Vector2(monsterHealth.Monster.Position.X, monsterHealth.Monster.Position.Y), Color.White);
        }

        public override void Update(GameTime p_gameTime)
        {
            gauge.Update(p_gameTime, (int)monsterHealth.Life, new Vector2(((int)(monsterHealth.Monster.Position.X - monsterHealth.Monster.monsterWidth / 2)), (((int)monsterHealth.Monster.Position.Y + monsterHealth.Monster.monsterHeight / 2) + 2)));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawLifeGauge(spriteBatch);
        }

        private void DrawLifeGauge(SpriteBatch p_SpriteBatch)
        {
            gauge.Draw(p_SpriteBatch);
        }
    }
}
