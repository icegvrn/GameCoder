using BricksGame.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
     abstract public class Scene
    {
        protected MainGame mainGame;
        protected List<IActor> listActors;

        public Scene(MainGame p_mainGame) { mainGame = p_mainGame; listActors = new List<IActor>(); }

        public virtual void Load() { }
        public virtual void UnLoad() { }
        public virtual void Update(GameTime gameTime) 
        {

            for (int i = listActors.Count - 1; i >= 0; i--)
            {
                listActors[i].Update(gameTime);

                foreach (IActor actor2 in listActors)
                {
                    if (actor2 != listActors[i])
                    {
                        if (listActors[i] is ICollider && actor2 is ICollider)
                        {
                           ICollider c_actor = (ICollider)listActors[i];
                            ICollider c_actor2 = (ICollider)actor2;
                            if (Utils.CollideByBox(listActors[i], actor2))
                            {
                                c_actor.TouchedBy(actor2);
                                c_actor2.TouchedBy(listActors[i]);
                            }
                        }

                       
                    }
                }
              
                if (listActors[i] is IDestroyable)
                {
                    IDestroyable dActor = (IDestroyable)listActors[i];
                    if (dActor.IsDestroy)
                    {
                        listActors.Remove(listActors[i]);
                    }
                }

            }
           
        }
        public virtual void Draw(GameTime gameTime) 
        {
            foreach (IActor actor in listActors)
            {
                actor.Draw(mainGame._spriteBatch);
            }

        }

        public void AddToActorList(IActor actor)
        {
            listActors.Add(actor);
        }

    }
}
