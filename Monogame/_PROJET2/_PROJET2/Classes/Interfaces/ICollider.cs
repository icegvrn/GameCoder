using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public interface ICollider
    {
        Vector2 Position { get; }
        Rectangle BoundingBox { get; }
        void TouchedBy(GameObject p_By);


    }
}