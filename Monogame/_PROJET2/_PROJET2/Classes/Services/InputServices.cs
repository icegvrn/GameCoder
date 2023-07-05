using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace BricksGame
{
    public class InputServices : IInputService
    {
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;
        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
        public bool OnActionReleased()
        {
           if (currentMouseState.LeftButton != previousMouseState.LeftButton && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
           else
            {
                return false;
            }
        }

        public bool OnActionDown()
        {
            if ((IsLeftMouseButtonDown()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool OnSecondaryActionDown()
        {
            if ((IsMouseRightButtonDown()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool OnSecondaryActionReleased()
        {
            if (currentMouseState.RightButton != previousMouseState.RightButton && currentMouseState.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool OnReturnReleased()
        {
            return IsKeyReleased(Keys.Escape);
        }

        public bool OnPauseReleased()
        {
            return IsKeyReleased(Keys.Space);
        }

        public bool OnLeftReleased()
        {
            if (IsKeyReleased(Keys.Q) || IsKeyReleased(Keys.Left))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool OnRightReleased()
        {
            if (IsKeyReleased(Keys.D) || IsKeyReleased(Keys.Right))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool OnLeftDown()
        {
            if (IsKeyDown(Keys.Q) || IsKeyDown(Keys.Left))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool OnRightDown()
        {
            if (IsKeyDown(Keys.D) || IsKeyDown(Keys.Right))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Vector2 GetMousePosition()
        {
            MouseState currentMouseState = Mouse.GetState();
            return new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        public bool IsKeyDown(Keys key)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            return currentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            return currentKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return GameKeyboard.IsKeyReleased(key);
        }

        public bool IsLeftMouseButtonDown()
        {
            MouseState currentMouseState = Mouse.GetState();
           
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsMouseRightButtonDown()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsLeftMouseButtonUp()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsMouseRightButtonUp()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.RightButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public MouseState CurrentMouseState()
        {
            return currentMouseState;
        }

        public KeyboardState CurrentKeyboardState()
        {
            return currentKeyboardState;
        }

        public MouseState LastMouseState()
        {
            return previousMouseState;
        }

        public KeyboardState LastKeyboardState()
        {
            return previousKeyboardState;
        }




        public void InputUpdateBegin()
        {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }

        public void InputUpdateEnd()
        {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }
     

         
        }
}
