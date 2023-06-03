using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBrique
{
    internal class bBubbleBrique : Brique
    {
        public override void Tape()
        {
            Debug.WriteLine("BUBBLEBRIQUE : Je bulle toute la journée !");
        }
    }
}
