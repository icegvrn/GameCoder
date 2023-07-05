using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    public class AssetsManagerService : IAssetsServices
    {
        public Texture2D blankTexture { get; private set; }

       public AssetsManagerService(ContentManager content)
        {
            blankTexture = content.Load<Texture2D>("images/blank");
        }

        public Texture2D GetGameTexture(IAssetsServices.textures texture)
        {
            switch(texture)
            {
                case IAssetsServices.textures.blank:
                    return blankTexture;
                default:
                    return blankTexture;
            }
        }
       
    }
}
