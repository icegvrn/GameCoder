using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{ 
    public interface IAssetsServices
    {
    enum textures {  blank }
    Texture2D  GetGameTexture(textures texture);
    }
}
