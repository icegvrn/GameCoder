using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public interface IPathsService
    {
  
        string GetImagesRoot();
        string GetMonstersImagesPathRoot();
        string GetPlayerImagesPathRoot();
        string GetDicesImagesPathRoot();
        string GetSoundsRoot();

        string GetBallSoundsRoot();
        string GetMonstersSoundsRoot();
        string GetPlayerSoundsRoot();
        string GetMusicsSoundsRoot();
        string GetJSONRoot();

        string GetJSONGameLevelPath();
        string GetJSONSavedLevelPath();

    }
}
