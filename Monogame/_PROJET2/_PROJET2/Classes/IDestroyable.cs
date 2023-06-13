using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    internal interface IDestroyable
    {
        bool IsDestroy { get; set; }
        void Destroy();
    }
}
