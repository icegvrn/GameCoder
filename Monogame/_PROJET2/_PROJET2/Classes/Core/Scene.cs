using BricksGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BricksGame
{
     abstract public class Scene
    {
        protected MainGame mainGame;
        protected List<GameObject> gameObjectsList;
        protected Texture2D background;
        public Scene(MainGame p_mainGame) {
            mainGame = p_mainGame; 
            gameObjectsList = new List<GameObject>(); 
        }

        public virtual void Load() { }
        public virtual void UnLoad() { }
        public virtual void Update(GameTime gameTime) 
        {
            RegisterDestroyedGameObjects();
            RegisterCollisions();
            UpdateGameObjects(gameTime);   
        }

        public virtual void Draw(GameTime gameTime) 
        {
            DrawAllGameObjects();
        }

        public void AddToGameObjectsList(GameObject gameObj)
        {
            gameObjectsList.Add(gameObj);
        }

        public void RemoveToGameObjectsList(GameObject gameObj)
        {
            gameObjectsList.Remove(gameObj);
        }

        public void UpdateGameObjects(GameTime gameTime)
        {
            for (int i = gameObjectsList.Count - 1; i >= 0; i--)
            {
                gameObjectsList[i].Update(gameTime);
            }
        }

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
                              //  c_colliderObject2.TouchedBy(gameObjectsList[i]);
                            }
                        }
                    }
                }
             
            }
        }


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

        public void DrawAllGameObjects()
        {
            foreach (GameObject gameObj in gameObjectsList)
            {
                gameObj.Draw(mainGame._spriteBatch);
            }
        }

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

        public virtual void End()
        {

        }

     

    }
}
