using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class PlayerInput
    {
        private Player player;
        private MouseState oldMouseState;
        public bool CanMove;
        public PlayerInput(Player p_player)
        {
         player = p_player;
         CanMove = true;
        }

        public void Update(GameTime gameTime)
        {
            if (player.IsReady && CanMove)
            {
            MouseState newMouseState = Mouse.GetState();
            Point mousePos = newMouseState.Position;

            // Gestion des mouvements du joueur
            if (Keyboard.GetState().IsKeyDown(Keys.Q) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                player.Left();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                player.Right();
            }
            else
            {
                player.Stay();
            }

            if (GameKeyboard.IsKeyReleased(Keys.Q) || GameKeyboard.IsKeyReleased(Keys.Left))
            {
                player.Stay();
            }
            else if (GameKeyboard.IsKeyReleased(Keys.D) || GameKeyboard.IsKeyReleased(Keys.Left))
            {
                player.Stay();
            }

            // Gestion des actions du joueur
            if ((newMouseState != oldMouseState && newMouseState.LeftButton == ButtonState.Pressed) || GameKeyboard.IsKeyReleased(Keys.Space))
            {
                player.Action();
            }

            oldMouseState = newMouseState;
        }
        }
    }
}

