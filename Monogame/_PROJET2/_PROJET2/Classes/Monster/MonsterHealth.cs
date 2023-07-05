using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Threading;

namespace BricksGame
{
    public class MonsterHealth : Health
    {
    
        public MonsterHealthUI monsterHealthUI;
        public Monster Monster { get; private set; }

        public MonsterHealth(Monster p_monster, int lvl)
        {   
            Monster = p_monster;
            InitLife(lvl);
            monsterHealthUI = new MonsterHealthUI(this);
    }

        public override void Update(GameTime p_gameTime)
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            monsterHealthUI.Draw(spriteBatch);
        }

        public override void InitLife(float lvl)
        {
            Life = lvl * 50;
            InitialLife = Life;
            ProvisoryLife = Life;
        }

        public override void Damage(float lifeFactor)
        {
            ProvisoryLife = Life - lifeFactor;
        }

        public void Kill()
        {
            Life = 0;
        }



    }
}
