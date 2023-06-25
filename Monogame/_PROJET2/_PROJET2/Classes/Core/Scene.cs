using BricksGame;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace BricksGame
{
     abstract public class Scene
    {
        protected MainGame mainGame;
        protected List<GameObject> gameObjects;

        public Scene(MainGame p_mainGame) { mainGame = p_mainGame; gameObjects = new List<GameObject>(); }

        public virtual void Load() { }
        public virtual void UnLoad() { }
        public virtual void Update(GameTime gameTime) 
        {

            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                gameObjects[i].Update(gameTime);

                foreach (GameObject gameObj2 in gameObjects)
                {
                    if (gameObj2 != gameObjects[i])
                    {
                        if (gameObjects[i] is ICollider && gameObj2 is ICollider)
                        {
                           ICollider c_colliderObject = (ICollider)gameObjects[i];
                            ICollider c_colliderObject2 = (ICollider)gameObj2;
                            if (Utils.CollideByBox(gameObjects[i], gameObj2))
                            {
                                c_colliderObject.TouchedBy(gameObj2);
                                c_colliderObject2.TouchedBy(gameObjects[i]);
                            }
                        }  
                    }
                }
              
                if (gameObjects[i] is IDestroyable)
                {
                    IDestroyable dActor = (IDestroyable)gameObjects[i];
                    if (dActor.IsDestroy)
                    {
                        gameObjects.Remove(gameObjects[i]);
                    }
                }
            }    
        }

        public virtual void Draw(GameTime gameTime) 
        {
            foreach (GameObject gameObj in gameObjects)
            {
                gameObj.Draw(mainGame._spriteBatch);
            }

        }

        public void AddToGameObjectsList(GameObject gameObj)
        {
            gameObjects.Add(gameObj);
        }

        public void RemoveToGameObjectsList(GameObject gameObj)
        {
            gameObjects.Remove(gameObj);
        }

        public bool IsSceneContainsObjectTypeOf<T>()
        {
            foreach (GameObject gameObj in gameObjects)
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
