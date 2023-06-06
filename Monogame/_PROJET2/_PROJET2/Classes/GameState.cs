using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
     public class GameState
    {
        public enum SceneType
        {
            Menu,
            Gameplay,
            GameOver
        }
        public Scene CurrentScene { get; private set; }

        protected MainGame mainGame;
    

        public GameState(MainGame p_mainGame)
        {
            mainGame = p_mainGame;
        }

        public void ChangeScene(SceneType p_sceneType)
        {
            if (CurrentScene != null)
            {
                CurrentScene.UnLoad();
                CurrentScene = null;
            }

            switch (p_sceneType)
            {
                case SceneType.Menu:
                    CurrentScene = new SceneMenu(mainGame);
                    break;
                case SceneType.Gameplay:
                    CurrentScene = new SceneGameplay(mainGame);
                    break;
                case (SceneType.GameOver):
                    CurrentScene = new SceneGameOver(mainGame);
                    break;
                default:
                break;

            }
            CurrentScene.Load();
        }

    }
}
