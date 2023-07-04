using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace BricksGame
{
    public class GameManager
    {
        Scene currentScene;
        private Player player;
        private ContentManager content;
        LevelManager levelManager;
        public bool IsGameWin;
        private SoundEffect sndWinLevel;
        public GameManager()
        {
            currentScene = ServiceLocator.GetService<GameState>().CurrentScene;
            content = ServiceLocator.GetService<ContentManager>();
            sndWinLevel = content.Load<SoundEffect>("Sounds/nextLevel");
        }

        public void Load()
        {
            LoadNewPlayer();
            LoadLevelManager();
        }


        public void Update(GameTime gameTime)
        {   
            
            if (GameKeyboard.IsKeyReleased(Keys.M))
            {
                levelManager.GameGrid.Down();
            }

            if (levelManager.currentState == LevelManager.LevelState.play)
            {
                foreach (Ball ball in player.playerFighter.BallsList)
                {
                   ball.CheckCollision(levelManager.GameGrid.GridElements);
                    ball.CheckCollision(player);
                }
               
            }   
            
            CheckPlayerDeath();
            RegisterInput();
            ManageMonstersAttack();
            UpdateCurrentLevel(gameTime);
            DoEventsOnLevelState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            levelManager.GameGrid.DrawGrid(spriteBatch);
            //levelManager.GameGrid.DrawSlots(spriteBatch);
        }

        private void LoadNewPlayer()
        {
            player = new Player(content.Load<Texture2D>("images/pad"));
            currentScene.AddToGameObjectsList(player);
        }

        private void LoadLevelManager()
        {
            levelManager = new LevelManager();
            levelManager.LoadLevel(1);
            ServiceLocator.RegisterService(levelManager);
        }

        public void NextLevel()
        {
            levelManager.NextLevel();
            player.IsReady = false;
            player.Reset();
            player.playerFighter.ResetMunition();
        }

        private void RegisterInput()
        {
            if (GameKeyboard.IsKeyReleased(Keys.W))
            {
                NextLevel();
            }
        }

        private void ManageMonstersAttack()
        {
            foreach (IBrickable brick in levelManager.GameGrid.GridElements)
            {
                if (brick is Monster)
                {
                    Monster monster = (Monster)brick;
                    if (monster.Position.Y >= levelManager.GameGrid.maxDestination && !monster.IsDead)
                    {
                       monster.Attack();
                        if (monster.Fighter.IsAttacker)
                        {
                        player.IsHit(monster, monster.Fighter.Power);
                        }
                    }
                }
            }
        }
        private void CheckPlayerDeath()
        {
            if (player.IsDead)
            {
                levelManager.currentState = LevelManager.LevelState.gameOver;
                currentScene.End();
            }
        }
        private void UpdateCurrentLevel(GameTime gameTime)
        {
            levelManager.Update(gameTime);
        }
        private void DoEventsOnLevelState()
        {
            DoEventsOnStateDices();
            DoEventsOnStatePlay();
            DoEventsOnStateWin();
        }




        private void DoEventsOnStateDices()
        {
            if (levelManager.currentState == LevelManager.LevelState.dices)
            {
                player.ChangeState(Gamesystem.CharacterState.idle);
            }
        }

        private void DoEventsOnStatePlay()
        {
            if (levelManager.currentState == LevelManager.LevelState.play)
            {
                SetPlayerReady();
                DoActionsIfNoBall();
            }
        }

        private void SetPlayerReady()
        {
            if (!player.IsReady)
            {
                player.IsReady = true;
                player.Prepare();
            }
        }

        private void DoActionsIfNoBall()
        {
            if (!currentScene.IsSceneContainsObjectTypeOf<Ball>())
            {
                if (player.playerFighter.HasMunition)
            {
                    levelManager.NoBallActions();
                    player.Reset();
                    player.Prepare();
              
            }
            else
            {
                    levelManager.currentState = LevelManager.LevelState.gameOver;
                    currentScene.End();
                }
            }

        }

        private void DoEventsOnStateWin()
        {
            if (levelManager.currentState == LevelManager.LevelState.win)
            {
                NextLevel();
                sndWinLevel.Play();
            }

            if (levelManager.currentState == LevelManager.LevelState.end)
            {
                IsGameWin = true;
            }

           
        }
    }
}