using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public interface ISessionService
    {
         int GetPoints();

         int GetMaxPoints();

         int GetLife();
         void SetLife(int p_life);


         void SetPoints(int p_points);

         void SetMaxPoints(int p_points);


         void AddLife(int p_life);



         void SubsLife(int p_damage);


         void AddPoints(int p_points);



         void SubsPoints(int p_points);


         void MultiplyPoints(int p_multiplicator);

         void MultiplyPoints(int p_points, int p_multiplicator);


         void DividePoints(int p_divider);
    }
}
