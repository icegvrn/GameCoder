using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBrique
{
    internal class bExplosingBrique : Brique
    {
        public override void Tape()
        {
            Debug.WriteLine("EXPLOSINGBRIQUE : J'explose du tonnerre !");
        }
    }
}
