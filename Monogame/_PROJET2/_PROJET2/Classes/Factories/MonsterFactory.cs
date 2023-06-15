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
        List<Texture2D> monsterTextures;
        public Monster CreateMonster(int lvl)
        {
             monsterTextures = new List<Texture2D>();
            ContentManager content = ServiceLocator.GetService<ContentManager>();

            if (content.Load<Texture2D>("images/Monsters/" + lvl) != null)
            {
                monsterTextures.Add(content.Load<Texture2D>("images/Monsters/" + lvl+""));
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
