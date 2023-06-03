using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBrique
{
    internal class bFireBrique : Brique
    {
        public override void Tape()
        {
            Debug.WriteLine("FIREBRIQUE : Je crache du feu de dieu !");
        }
    }
}
