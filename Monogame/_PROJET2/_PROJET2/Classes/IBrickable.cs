using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame.Classes
{
    public interface IBrickable
    {
       Vector2 Position { get; set; }
    }
}
