using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    internal interface IInputService
    {
        bool OnActionReleased();
        bool OnActionDown();
        bool OnSecondaryActionDown();
        bool OnSecondaryActionReleased();
        bool OnReturnReleased();
        bool OnPauseReleased();
        bool OnLeftReleased();
        bool OnRightReleased();
        bool OnLeftDown();
        bool OnRightDown();
        Vector2 GetMousePosition();
        bool IsKeyDown(Keys key);
        bool IsKeyUp(Keys key);
        bool IsKeyReleased(Keys key);
        bool IsLeftMouseButtonDown();
        bool IsMouseRightButtonDown();
        bool IsLeftMouseButtonUp();
        bool IsMouseRightButtonUp();
        MouseState CurrentMouseState();
        KeyboardState CurrentKeyboardState();
        MouseState LastMouseState();
        KeyboardState LastKeyboardState();

    }
}
