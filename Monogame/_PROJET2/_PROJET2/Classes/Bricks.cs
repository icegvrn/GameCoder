using BricksGame.Classes;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class Bricks : Sprite, ICollider, IDestroyable
    {
        private bool isDestroy;
        public bool IsDestroy { get { return isDestroy; } set { isDestroy = value; } }
        public Bricks(List<Texture2D> p_textures): base(p_textures) 
        {

        }
        public void TouchedBy(IActor p_By)
        {
            if (p_By is Ball)
            {
                Destroy();
            }
        }

        public void Destroy()
        {
            isDestroy = true;
        }
    }
}

