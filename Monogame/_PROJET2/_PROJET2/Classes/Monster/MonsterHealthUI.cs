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
        public class MonsterHealthUI
    {
        // Jauge de vie
        private MonsterHealth monsterHealth;
        private EvolutiveColoredGauge gauge;
      
        int lifeBarLenght = 56;
        int lifeBarHeight = 4;
        Color[] lifeColors = { new Color(51, 225, 51), Color.Yellow, Color.Orange, Color.Red };
        float[] threshold = { 0.65f, 0.55f, 0.35f };

        public MonsterHealthUI(MonsterHealth m_health)
        {
            monsterHealth = m_health;
         
            AddGauge();
        }

        private void AddGauge()
        {
            Rectangle lifeBar = new Rectangle((int)monsterHealth.Monster.Position.X - monsterHealth.Monster.BoundingBox.Width / 2, (int)monsterHealth.Monster.Position.Y, lifeBarLenght, lifeBarHeight);
            gauge = new EvolutiveColoredGauge(monsterHealth.InitialLife, lifeBar, Color.White, threshold, lifeColors, true, new Vector2(monsterHealth.Monster.Position.X, monsterHealth.Monster.Position.Y), Color.White);
        }

        public void Update (GameTime p_gameTime)
        {
            gauge.Update(p_gameTime, (int)monsterHealth.Life, new Vector2(((int)(monsterHealth.Monster.Position.X - monsterHealth.Monster.monsterWidth / 2)), (((int)monsterHealth.Monster.Position.Y + monsterHealth.Monster.monsterHeight / 2) + 2)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawLifeGauge(spriteBatch);
        }

        private void DrawLifeGauge(SpriteBatch p_SpriteBatch)
        {
            gauge.Draw(p_SpriteBatch);

   
        }
    }
}
