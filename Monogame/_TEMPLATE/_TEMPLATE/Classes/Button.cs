using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _TEMPLATE
{
    public delegate void OnClick(Button p_Sender);
    public delegate void OnHover(Button p_Sender);
    public class Button : Sprite
    {
        public bool isHover { get; private set; }
        private MouseState oldMouseState;
   
        public OnClick onClick { get; set; }
        public OnHover onHover { get; set; }
        public Button(List<Texture2D> p_texture) : base(p_texture)
        {
        }


        public override void Update (GameTime gameTime) {
            MouseState newMouseState = Mouse.GetState();
            Point MousePos = newMouseState.Position;

            if (BoundingBox.Contains(MousePos))
            {
                if (!isHover)
                {
                    isHover = true;
                    changeSprite(1);
                    Debug.WriteLine("The button is now Hover");
                }
                else
                {
                    if (newMouseState != oldMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                    {               
                        Debug.WriteLine("Clic !");
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
            oldMouseState = newMouseState;
            base.Update(gameTime);
        }

        private void changeSprite(int nb)
        {
            if (Textures[nb] != null)
            {
                currentTexture = Textures[nb];
            }
          
        }
    }
}
