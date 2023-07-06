using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace BricksGame
{
    /// <summary>
    /// Classe static permettant de vérifier qu'une touche a été released et pas juste down
    /// </summary>
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
