using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{ 
    public interface IAssetsServices
    {
    enum textures {  blank }
    Texture2D  GetGameTexture(textures texture);
    }
}
