using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    public class MonsterFactory
    {
        public Monster CreateMonster(int lvl)
        {
            
            ContentManager content = ServiceLocator.GetService<ContentManager>();
            string imgPath = ServiceLocator.GetService<IPathsService>().GetMonstersImagesPathRoot();

            if (content.Load<Texture2D>(imgPath+"idle/" + lvl) != null)
            {
               
              Monster newMonster = new Monster(content.Load<Texture2D>(imgPath+"idle/" + lvl), lvl);

                return newMonster;
            }
            else
            {
                return null;
            }
        }
    }
}
