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
               
              Monster newMonster = new Monster(content.Load<Texture2D>("images/Monsters/idle/" + lvl), lvl);

                return newMonster;
            }
            else
            {
                return null;
            }
        }
    }
}
