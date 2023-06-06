using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame.Classes
{
    internal interface IDestroyable
    {
        bool IsDestroy { get; set; }
        void Destroy();
    }
}
