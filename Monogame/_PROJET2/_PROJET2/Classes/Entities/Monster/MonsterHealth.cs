using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace BricksGame
{
    public class MonsterHealth : Health
    {
        /// <summary>
        /// Classe qui gère le niveau de vie du monstre
        /// </summary>
        /// 
        private MonsterHealthUI monsterHealthUI;
        public Monster Monster { get; private set; }

        public MonsterHealth(Monster p_monster, int lvl)
        {   
            Monster = p_monster;
            InitLife(lvl);
            monsterHealthUI = new MonsterHealthUI(this);
    }

        //Update de la vie : animation progressive de la vie qui descend si le monstre a été touché, mise à mort s'il est à 0 de points de vie
       // Update de la barre de vie via monsterHealthUI
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

        // Dessin de la barre de vie via monsterHealthUI
        public override void Draw(SpriteBatch spriteBatch)
        {
            monsterHealthUI.Draw(spriteBatch);
        }

        // Initialisation de la vie du monstre en fonction de son level
        public override void InitLife(float lvl)
        {
            Life = lvl * 50;
            InitialLife = Life;
            ProvisoryLife = Life;
        }

        // Méthode permettant de mettre des damage au monstre
        public override void Damage(float lifeFactor)
        {
            ProvisoryLife = ProvisoryLife - lifeFactor;
        }

        // Méthode permettant de tuer le monstre en le mettant à 0 pv.
        public void Kill()
        {
            Life = 0;
        }

    }
}
