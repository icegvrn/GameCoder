using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    internal interface IParticulable
    {
       public List<TimedParticles> timedParticles { get; set; }
    }
}
