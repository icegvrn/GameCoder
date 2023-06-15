using BricksGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace BricksGame
{
    public class GameManager
    {
        Scene currentScene;
        private Ball ball;
        private List<TimedParticles> ballTrail;
        private Pad pad;
        private ContentManager content;
        private BricksList brickList;
        private Dice dice;
        LevelManager levelManager;
        public GameManager(Scene p_currentScene)
        {
            currentScene = p_currentScene;
            content = ServiceLocator.GetService<ContentManager>();
            ServiceLocator.RegisterService(currentScene);
       
        }

        public void Load()
        {
           CreateNewPad();
          CreateNewBall();
          levelManager = new LevelManager(this);
            ServiceLocator.RegisterService(levelManager);
            levelManager.LoadLevel(1);
       
        }

        public void RegisterActor(GameObject actor)
        {
            currentScene.AddToGameObjectsList(actor);
        }

        public void CreateNewBall()
        {
            ballTrail = new List<TimedParticles>();
            List<Texture2D> myBallTextureList = new List<Texture2D>();
            myBallTextureList.Add(content.Load<Texture2D>("images/ball"));
            ball = new Ball(myBallTextureList);
            ball.Position = new Vector2(pad.Position.X + pad.currentTexture.Width / 2 - ball.currentTexture.Width / 2, pad.Position.Y - pad.currentTexture.Height / 2);
            ball.Following(pad);
            currentScene.AddToGameObjectsList(ball);
        }

        public void CreateNewPad()
        {
           
            List<Texture2D> myPadTextureList = new List<Texture2D>();
            myPadTextureList.Add(content.Load<Texture2D>("images/pad"));
            pad = new Pad(myPadTextureList);
            pad.Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - pad.currentTexture.Width / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - pad.currentTexture.Height * 2);
            currentScene.AddToGameObjectsList(pad);
        }

        public void Update(GameTime gameTime)
        {
            levelManager.Update(gameTime);

            if (levelManager.currentState == LevelManager.LevelState.play)
            {

                for (int n = ballTrail.Count - 1; n >= 0; n--)
                {
                    if (ballTrail[n].timer <= 0)
                    {
                        ballTrail.Remove(ballTrail[n]);
                    }
                    else
                    {
                        ballTrail[n].timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }



                if (!currentScene.IsSceneContainsObjectTypeOf<Ball>())
                {
                    levelManager.NoBallActions();
                    CreateNewBall();
                }

                if (GameKeyboard.IsKeyReleased(Keys.Space))
                {
                    ball.Fire();

                }

                if (GameKeyboard.IsKeyReleased(Keys.O))
                {
                    //  dice.RollDice();  
                }


                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    pad.Move(-1, 0);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    pad.Move(1, 0);
                }


            }



        }
    }
}
