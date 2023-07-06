using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BricksGame
{
    /// <summary>
    /// Le GameManager est responsable de la gestion globale du jeu. Il contrôle le chargement des niveaux via levelManager, la création du joueur, la mise à jour des événements du jeu et la gestion de l'état du niveau.
    /// </summary>
    public class GameManager
    {
        // Le GameManager demande au levelManager de charger un level et crée un joueur
        LevelManager levelManager;
        private Player player;

        // Il vérifie si le jeu est gagné
        public bool IsGameWin;
        private SoundEffect sndWinLevel;

        // Composants
        private Scene currentScene;
        private ContentManager content;
        

        public GameManager()
        {
            currentScene = ServiceLocator.GetService<GameState>().CurrentScene;
            content = ServiceLocator.GetService<ContentManager>();
            string soundPath = ServiceLocator.GetService<IPathsService>().GetSoundsRoot();
            sndWinLevel = content.Load<SoundEffect>(soundPath + "nextLevel");
        }

        // En GameMode, on charge un level et crée un nouveau joueur
        public void Load()
        {
            LoadNewPlayer();
            LoadLevelManager();
        }


        public void Update(GameTime gameTime)
        {
            // Update des events majeurs de jeu
            CheckBallCollisions();
            ManageMonstersAttack();
            CheckPlayerDeath();

            // Update du level courant
            UpdateCurrentLevel(gameTime);
            DoEventsOnLevelState();

            DebugInputEnable();
        }

        // On appele le draw du levelManager
        public void Draw(SpriteBatch spriteBatch)
        {
            levelManager.Draw(spriteBatch);
        }


        private void DoEventsOnLevelState()
        {
            DoEventsOnStateDices();
            DoEventsOnStatePlay();
            DoEventsOnStateWin();
        }

        // Phase de dés, le joueur est mis en IDLE.
        private void DoEventsOnStateDices()
        {
            if (levelManager.CurrentState == LevelManager.LevelState.dices)
            {
                player.ChangeState(Gamesystem.CharacterState.idle);
            }
        }

        // Phase de jeu, on vérifie si y'a encore une balle en jeu et on reset le joueur au besoin.
        private void DoEventsOnStatePlay()
        {
            if (levelManager.CurrentState == LevelManager.LevelState.play)
            {
                SetPlayerReady();
                DoActionsIfNoBall();
            }
        }

        // Phase de win : si level fini mais pas jeu, on change de level, sinon on passe un booléan qui va interagir la SceneGameplay
        private void DoEventsOnStateWin()
        {
            if (levelManager.CurrentState == LevelManager.LevelState.win)
            {
                NextLevel();
                sndWinLevel.Play();
            }

            if (levelManager.CurrentState == LevelManager.LevelState.end)
            {
                IsGameWin = true;
            }
        }

        // Création d'un nouveau joueur et ajout dans les gameObjects
        private void LoadNewPlayer()
        {
            player = new Player(content.Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetPlayerImagesPathRoot()+"pad"));
            currentScene.AddToGameObjectsList(player);
        }

        // Au chargement du levelManager, on charge le level 1
        private void LoadLevelManager()
        {
            levelManager = new LevelManager();
            levelManager.LoadLevel(1);
            ServiceLocator.RegisterService(levelManager);
        }

        // Reset du joueur au changement de niveau
        public void NextLevel()
        {
            levelManager.NextLevel();
            player.ResetAll();
        }

        // Phase de jeu : on vérifie la collision des balles
        private void CheckBallCollisions()
        {
            if (levelManager.CurrentState == LevelManager.LevelState.play)
            {
                foreach (Ball ball in player.playerFighter.BallsList)
                {
                    ball.CollisionManager.CheckCollision(levelManager.GameGrid.GridElements);
                    ball.CollisionManager.CheckCollision(player);
                }

            }
        }

        // Phase de jeu : on vérifie si des monstres sont attaquants ou non
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
                            player.IsHit(monster, monster.Fighter.Strenght);    
                        }
                    }
                }
            }
        }

     // Phase de jeu : on vérifie si le joueur est mort ou pas
        private void CheckPlayerDeath()
        {
            if (player.IsDead)
            {
                levelManager.CurrentState = LevelManager.LevelState.gameOver;
                currentScene.End();
            }
        }
        private void UpdateCurrentLevel(GameTime gameTime)
        {
            levelManager.Update(gameTime);
        }

        private void SetPlayerReady()
        {
            player.SetReady();
        }

        // S'il n'y a pu de balle en jeu, soit on en crée une nouvelle, soit le joueur n'a plus de munition et on perd
        private void DoActionsIfNoBall()
        {
            if (!currentScene.IsSceneContainsObjectTypeOf<Ball>())
            {
                if (player.playerFighter.HasMunition)
                {
                    levelManager.OnNoBallInGame();
                    player.Reset();
                    player.Prepare();

                }
                else
                {
                    levelManager.CurrentState = LevelManager.LevelState.gameOver;
                    currentScene.End();
                }
            }

        }

        // Permet de passer de niveau rapidement en mode debug
        private void DebugInputEnable()
        {
            if (ServiceLocator.GetService<IInputService>().IsKeyReleased(Keys.W))
            {
                NextLevel();
            }
        }
    }
}