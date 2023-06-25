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
   
        private Player player; 
        private ContentManager content;
        LevelManager levelManager;
        public GameManager(Scene p_currentScene)
        {
            currentScene = p_currentScene;
            content = ServiceLocator.GetService<ContentManager>();
            ServiceLocator.RegisterService(currentScene);
        }

        public void Load()
        {
          CreateNewPlayer();
          levelManager = new LevelManager();
          ServiceLocator.RegisterService(levelManager);
          levelManager.LoadLevel(1);
        }
 
        public void CreateNewPlayer()
        {
            List<Texture2D> myPadTextureList = new List<Texture2D>();
            myPadTextureList.Insert((int)Gamesystem.CharacterState.idle, content.Load<Texture2D>("images/pad"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.l_idle, content.Load<Texture2D>("images/pad_left"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.walk, content.Load<Texture2D>("images/pad_walk"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.l_walk, content.Load<Texture2D>("images/pad_walk_left"));
            myPadTextureList.Insert((int)Gamesystem.CharacterState.fire, content.Load<Texture2D>("images/pad_attack"));

            player = new Player(myPadTextureList);
            currentScene.AddToGameObjectsList(player);
        }

        public void Update(GameTime gameTime)
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

            if (player.IsDead)
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
           if (!player.IsReady)
                {
                    player.IsReady = true;
                    player.Prepare();
                }

                if (!currentScene.IsSceneContainsObjectTypeOf<Ball>())
                {
                    levelManager.NoBallActions();
                    player.Reset();
                    player.Prepare();
                }

            }

            else if (levelManager.currentState == LevelManager.LevelState.win)
            {
                NextLevel();
            }
        }

        public void NextLevel()
        {
            levelManager.NextLevel();
            player.Reset();
            player.IsReady = false;
           
        }
    }
}
