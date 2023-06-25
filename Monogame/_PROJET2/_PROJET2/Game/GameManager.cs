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
       
            if (!pad.HasMunition)
            {
                currentScene.End();
            }

            ballTrail = new List<TimedParticles>();
            List<Texture2D> myBallTextureList = new List<Texture2D>();
            myBallTextureList.Add(content.Load<Texture2D>("images/ball"));
            ball = new Ball(myBallTextureList);
            ball.Position = new Vector2(pad.Position.X + pad.Size.X/2, pad.Position.Y- 20f);
            ball.Following(pad);
            currentScene.AddToGameObjectsList(ball);
        }

        public void CreateNewPad()
        {
            List<Texture2D> myPadTextureList = new List<Texture2D>();
            myPadTextureList.Insert((int)Gamesystem.CharacterState.idle, content.Load<Texture2D>("images/pad"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.l_idle, content.Load<Texture2D>("images/pad_left"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.walk, content.Load<Texture2D>("images/pad_walk"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.l_walk, content.Load<Texture2D>("images/pad_walk_left"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.fire, content.Load<Texture2D>("images/pad_attack"));

            pad = new Pad(myPadTextureList);
            currentScene.AddToGameObjectsList(pad);
        }

        public void Update(GameTime gameTime)
        {
            if (pad.IsDead)
            {
                currentScene.End();
            }

            levelManager.Update(gameTime);

            if (GameKeyboard.IsKeyReleased(Keys.W))
            {
                NextLevel();
            }

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

                if (GameKeyboard.IsKeyReleased(Keys.O))
                {
                    //  dice.RollDice();  
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    pad.Move(-1, 0);
                    pad.ChangeState(Gamesystem.CharacterState.l_walk);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    pad.Move(1, 0);
                    pad.ChangeState(Gamesystem.CharacterState.walk);
                }
                else if (GameKeyboard.IsKeyReleased(Keys.Space))
                {   
                    ball.Fire();
                    pad.RemoveMunition(1);
                    pad.ChangeState(Gamesystem.CharacterState.idle);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    pad.ChangeState(Gamesystem.CharacterState.fire);
                }
                if (GameKeyboard.IsKeyReleased(Keys.Q))
                {
                    pad.ChangeState(Gamesystem.CharacterState.l_idle);
                }
                else if (GameKeyboard.IsKeyReleased(Keys.D))
                {
                    pad.ChangeState(Gamesystem.CharacterState.idle);
                }
            }

            else if (levelManager.currentState == LevelManager.LevelState.win)
            {
                NextLevel();
            }
        }

        public void NextLevel()
        {
            ball.Destroy();
            levelManager.NextLevel();
            CreateNewBall();
            pad.Reset();
        }
    }
}
