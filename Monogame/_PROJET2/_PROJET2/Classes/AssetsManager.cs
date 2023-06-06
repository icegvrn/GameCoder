using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    internal class AssetsManager
    {
        public static SpriteFont MainFont { get; private set; }
        public static Song menuMusic { get; private set; }
        public static Song gamePlayMusic { get; private set; }
        public static void Load(ContentManager content) 
        {
            Debug.WriteLine("ASSETMANAGER : JE PASSE");
            menuMusic = content.Load<Song>("cool");
            gamePlayMusic = content.Load<Song>("techno");
            MainFont = content.Load<SpriteFont>("font");
        }

    }
}
