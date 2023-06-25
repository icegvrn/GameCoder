using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BricksGame
{
    public static class GameKeyboard
    {
        private static Dictionary<Keys, KeyboardState> keyStates = new Dictionary<Keys, KeyboardState>();

        public static bool IsKeyReleased(Keys key)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (!keyStates.ContainsKey(key))
            {
                keyStates[key] = currentKeyboardState;
                return false;
            }

            if (currentKeyboardState.IsKeyUp(key) && keyStates[key].IsKeyDown(key))
            {
                keyStates[key] = currentKeyboardState;
                return true;
            }

            keyStates[key] = currentKeyboardState;
            return false;
        }
    }
}
