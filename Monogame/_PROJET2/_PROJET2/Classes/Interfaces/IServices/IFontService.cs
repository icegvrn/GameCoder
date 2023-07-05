using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public interface IFontService
    {
        enum Fonts { MainFont, Font10, Font14 }
        SpriteFont GetFont(Fonts myFont);
    }
}
