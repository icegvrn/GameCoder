using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
     public class FontService : IFontService
    {

        SpriteFont mainFont;
        SpriteFont font10;
        SpriteFont font14;
         public FontService(ContentManager content)
        {
            mainFont = content.Load<SpriteFont>("font");
            font10 = content.Load<SpriteFont>("font10");
            font14 = content.Load<SpriteFont>("font14");
        }
        
        public SpriteFont GetFont(IFontService.Fonts myFont)
        {
           switch (myFont)
            {
                case IFontService.Fonts.Font10:
                    return font10;
                case IFontService.Fonts.Font14:
                    return font14;
                case IFontService.Fonts.MainFont:
                    return mainFont;
                default:
                    return mainFont;
            }
        }
    }
}
