using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    public interface IFontService
    {
        enum Fonts { MainFont, Font10, Font14 }
        SpriteFont GetFont(Fonts myFont);
    }
}
