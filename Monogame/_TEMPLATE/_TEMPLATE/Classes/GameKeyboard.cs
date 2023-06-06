using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _TEMPLATE
{
    public static class GameKeyboard
    {
        private static KeyboardState newKeyboardState;
        private static KeyboardState oldKeyboardState;

        public static bool IsKeyReleased(Keys key)
        {
            newKeyboardState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(key))
            {
                if (oldKeyboardState != newKeyboardState) {
                    oldKeyboardState = newKeyboardState;
                    return true;
                }
                else
                {
                    oldKeyboardState = newKeyboardState;
                    return false;
                }
            }
            else
            {
                oldKeyboardState = newKeyboardState;
                return false;
            }
        }
    }
}
