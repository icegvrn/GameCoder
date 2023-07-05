using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BricksGame
{
    /// <summary>
    /// Classe abstraite qui sert de base pour les différentes scènes du jeu : contient le fait qu'on peut ajouter des gameObjects à une liste et qu'ils seront update et draw automatiquement
    /// </summary>
     abstract public class Scene
    {
        protected MainGame mainGame;
        protected List<GameObject> gameObjectsList;
        protected Texture2D background;
        public Scene(MainGame p_mainGame) 
        {
            mainGame = p_mainGame; 
            gameObjectsList = new List<GameObject>(); 
        }

        public virtual void Load() { }

        // Lorsqu'une scène se décharge, on supprime tous les gameObjects enregistrés pour qu'ils disparaissent de l'écran
        public virtual void UnLoad() 
        {
            for (int i = gameObjectsList.Count - 1; i >= 0; i--)
            {
                RemoveToGameObjectsList(gameObjectsList[i]);
            }

        }

        // Update tous les objets enregistrés de la scène, vérifie leur collision et s'ils ont été détruit
        public virtual void Update(GameTime gameTime) 
        {
            RegisterDestroyedGameObjects();
            RegisterCollisions();
            UpdateGameObjects(gameTime);   
        }

        // Dessine tous les objets enregistrés de la scène
        public virtual void Draw(GameTime gameTime) 
        {
            DrawAllGameObjects();
        }

        // Ajoute un gameObject à la scène en l'enregistrant : il sera update et draw en permanence
        public void AddToGameObjectsList(GameObject gameObj)
        {
            gameObjectsList.Add(gameObj);
        }

        // Supprime un gameObject de la liste
        public void RemoveToGameObjectsList(GameObject gameObj)
        {
            gameObjectsList.Remove(gameObj);
        }

        // Update tous les gameObjects enregitrés
        public void UpdateGameObjects(GameTime gameTime)
        {
                for (int i = 0; i <= gameObjectsList.Count - 1; i++)
                {
                if (gameObjectsList[i] != null)
                {
                    gameObjectsList[i].Update(gameTime);
                }
            }
           
        }

        // Vérifie les collisions de tous les objets enregistrés et appel leur fonction TouchBy
        public void RegisterCollisions()
        {
            for (int i = gameObjectsList.Count - 1; i >= 0; i--)
            {
                foreach (GameObject gameObj2 in gameObjectsList)
                {
                    if (gameObj2 != gameObjectsList[i])
                    {
                        if (gameObjectsList[i] is ICollider && gameObj2 is ICollider)
                        {
                            ICollider c_colliderObject = (ICollider)gameObjectsList[i];
                            ICollider c_colliderObject2 = (ICollider)gameObj2;
                            if (Utils.CollideByBox(gameObjectsList[i], gameObj2))
                            {
                                c_colliderObject.TouchedBy(gameObj2);
                            }
                        }
                    }
                }
             
            }
        }

        // Supprime un objet de la list de la scène si son bool IsDestroy est true
        public void RegisterDestroyedGameObjects()
        {
            for (int i = gameObjectsList.Count - 1; i >= 0; i--)
            {
                if (gameObjectsList[i] is IDestroyable)
                {
                    IDestroyable dActor = (IDestroyable)gameObjectsList[i];
                    if (dActor.IsDestroy)
                    {
                        gameObjectsList.Remove(gameObjectsList[i]);
                    }
                }
            }
        }

        // Dessine tous les gameObjects de la liste
        public void DrawAllGameObjects()
        {
            foreach (GameObject gameObj in gameObjectsList)
            {
                gameObj.Draw(mainGame._spriteBatch);
            }
        }

        // Permet de savoir si la scène continent un objet d'un certain type (utilisé pour la balle notamment)
        public bool IsSceneContainsObjectTypeOf<T>()
        {
            foreach (GameObject gameObj in gameObjectsList)
            {
                if (gameObj.GetType() == typeof(T))
                {
                    return true;
                }
            }
            return false;
           
        }

        // Utilisée pour les scènes qui ont "une fin"
        public virtual void End()
        {

        }

     

    }
}
