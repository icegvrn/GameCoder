using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using System.Linq;


namespace BricksGame
{
    public delegate void OnClick(Button p_Sender);
    public delegate void OnHover(Button p_Sender);
    public class Button : Sprite
    {
        public bool isHover { get; private set; }

        public OnClick onClick { get; set; }
        public OnHover onHover { get; set; }
        public Button(List<Texture2D> p_texture) : base(p_texture)
        {
        }


        public override void Update(GameTime gameTime)
        {
      

            if (BoundingBox.Contains(ServiceLocator.GetService<IInputService>().GetMousePosition()))
            {
                if (!isHover)
                {
                    isHover = true;
                    changeSprite(1);
                    onHover(this);
                }
                else
                {
                    if (ServiceLocator.GetService<IInputService>().OnActionReleased())
                    {
                        if (onClick != null)
                        {
                            onClick(this);
                        }
                    }
                }
            }
            else
            {
                isHover = false;
                changeSprite(0);
            }
  
            base.Update(gameTime);
        }

        private void changeSprite(int nb)
        {
            if (nb <= Textures.Count() - 1)
            {
                if (Textures[nb] != null)
                {
                    currentTexture = Textures[nb];
                }

            }
        }

        public void ChangeSpriteEffects(SpriteEffects sFX)
        {
            spriteEffects = sFX;
        }
    }
}
