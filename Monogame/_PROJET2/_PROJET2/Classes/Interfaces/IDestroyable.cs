using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public interface IDestroyable
    {
        bool IsDestroy { get; set; }
        void Destroy(IDestroyable destroyable);
        void Destroy();

    }
}
