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
        public static SpriteFont Font10 { get; private set; }
        public static SpriteFont Font14 { get; private set; }
        public static Song menuMusic { get; private set; }
        public static Song gamePlayMusic { get; private set; }

        public static Texture2D blankTexture { get; private set; }

        public static string levelsPath { get; private set; }
        public static string savedLevelsPath { get; private set; }
        public static string absoluteSavedLevelsPath { get; private set; }
        public static void Load(ContentManager content) 
        {
            menuMusic = content.Load<Song>("cool");
            gamePlayMusic = content.Load<Song>("background_gameplay");
            MainFont = content.Load<SpriteFont>("font");
            Font10 = content.Load<SpriteFont>("font10");
            Font14 = content.Load<SpriteFont>("font14");
            blankTexture = content.Load<Texture2D>("images/blank");
            levelsPath = "Content/Levels/levels.json";
            savedLevelsPath = "Content/Levels/savedLevel.json";
        }

    }
}
