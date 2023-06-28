﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BricksGame
{
    public class GameManager
    {
        Scene currentScene;
        private Player player; 
        private ContentManager content;
        LevelManager levelManager;
        public GameManager()
        {
            currentScene = ServiceLocator.GetService<GameState>().CurrentScene;
            content = ServiceLocator.GetService<ContentManager>();
        }

        public void Load()
        {
          LoadNewPlayer();
          LoadLevelManager();
        }
 
   
        public void Update(GameTime gameTime)
        {
            RegisterInput();
            ManageMonstersAttack();
            CheckPlayerDeath();
            UpdateCurrentLevel(gameTime);
            DoEventsOnLevelState();   
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
                    if (monster.isAttacking)
                    {
                        player.IsHit(monster, 20);
                    }
                }
            }
        }
        private void CheckPlayerDeath() 
        {
            if (player.IsDead)
            {
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
                levelManager.NoBallActions();
                player.Reset();
                player.Prepare();
            }
        }

        private void DoEventsOnStateWin()
        {
             if (levelManager.currentState == LevelManager.LevelState.win)
            {
                NextLevel();
            }
        }
    }
}
