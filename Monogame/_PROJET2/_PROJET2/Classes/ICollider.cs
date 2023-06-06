using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    internal interface ICollider
    {
        Vector2 Position { get; }
        Rectangle BoundingBox { get; }
        void TouchedBy(IActor p_By);
    }
}