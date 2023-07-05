
namespace BricksGame
{
    /// <summary>
    /// Content les différentes scènes du jeu sous forme d'enum et les méthodes pour changer de scène.
    /// </summary>
     public class GameState
    {
        public enum SceneType
        {
            Menu,
            Gameplay,
            GameOver, 
            Editor,
            Win
        }
        public Scene CurrentScene { get; private set; }

        protected MainGame mainGame;

        public enum GameMode { normal, saved };
        public GameMode currentGameMode;
        public bool hasASavedGame;
        public string currentLevelsJSON;
    

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
                case (SceneType.Editor):
                    CurrentScene = new SceneEditor(mainGame);
                    break;
                case (SceneType.Win):
                    CurrentScene = new SceneGameWin(mainGame);
                    break;
                default:
                break;

            }
            CurrentScene.Load();
        }

    }
}
