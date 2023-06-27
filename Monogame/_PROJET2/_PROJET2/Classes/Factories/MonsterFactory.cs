using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class MonsterFactory
    {
        public Monster CreateMonster(int lvl)
        {
            
            ContentManager content = ServiceLocator.GetService<ContentManager>();

            if (content.Load<Texture2D>("images/Monsters/idle/" + lvl) != null)
            {
                List<Texture2D>  monsterTextures = new List<Texture2D>();
                monsterTextures.Insert((int)Gamesystem.CharacterState.idle, content.Load<Texture2D>("images/Monsters/idle/" + lvl + ""));
                monsterTextures.Insert((int)Gamesystem.CharacterState.l_idle, null);
                monsterTextures.Insert((int)Gamesystem.CharacterState.walk, null);
                monsterTextures.Insert((int)Gamesystem.CharacterState.l_walk, null);
                monsterTextures.Insert((int)Gamesystem.CharacterState.fire, content.Load<Texture2D>("images/Monsters/attack/" + lvl + ""));
                monsterTextures.Insert((int)Gamesystem.CharacterState.die, content.Load<Texture2D>("images/Monsters/die/" + lvl + ""));
              Monster newMonster = new Monster(monsterTextures, lvl);

                return newMonster;
            }
            else
            {
                return null;
            }
        }
    }
}
