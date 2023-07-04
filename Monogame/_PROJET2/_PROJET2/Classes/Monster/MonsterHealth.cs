using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Threading;

namespace BricksGame
{
    public class MonsterHealth
    {
        public float InitialLife { get; private set; }
        public float ProvisoryLife { get; private set; }

        public float Life { get; private set; }
        public MonsterHealthUI monsterHealthUI;

        public bool IsDead { get; private set; }

        public Monster Monster { get; private set; }

        public MonsterHealth(Monster p_monster, int lvl)
        {   
            Monster = p_monster;
            InitLife(lvl);
            monsterHealthUI = new MonsterHealthUI(this);
    }

        public void Update(GameTime p_gameTime)
        {
            if (Life > 0)
            {
                if (Life > ProvisoryLife)
                {
                    Life -= 1;
                }
            }
            else
            {
                if (Monster.currentState != Gamesystem.CharacterState.die && !IsDead)
                {
                    IsDead = true;
                }
            }

            monsterHealthUI.Update(p_gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            monsterHealthUI.Draw(spriteBatch);
        }

        private void InitLife(int lvl)
        {
            Life = lvl * 50;
            InitialLife = Life;
            ProvisoryLife = Life;
        }


        public void RemoveLife(float lifeFactor)
        {
            ProvisoryLife = Life - lifeFactor;
        }

        public void Kill()
        {
            Life = 0;
        }

    }
}
