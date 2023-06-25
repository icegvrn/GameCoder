using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public interface IBrickable
    {
       Vector2 Position { get; set; }
        float Speed { get; set; }
        bool CanMove { get; set; }
        public virtual void Move(Vector2 destination) 
        {
            Position = destination;
        }
    }
}
