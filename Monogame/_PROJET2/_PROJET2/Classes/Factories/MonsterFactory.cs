using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    /// <summary>
    /// La MonsterFactory retourne un monstre à partir d'un level en valeurs int.
    /// </summary>
    public class MonsterFactory
    {
        // Créé un monstre en lui assignant un level et une image idle par défaut
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
