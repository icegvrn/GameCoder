using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class PathsService : IPathsService
    {
        public string GetImagesRoot()
        {
            return "images/";
        }

        public string GetMonstersImagesPathRoot()
        {
            return GetImagesRoot() + "Monsters/";
        }

        public string GetPlayerImagesPathRoot()
        {
            return GetImagesRoot() + "Player/";
        }

        public string GetDicesImagesPathRoot()
        {
            return GetImagesRoot() + "Dices/";
        }


        public string GetJSONRoot()
        {
            return "Content/Levels/";
        }

        public string GetJSONGameLevelPath()
        {
            return GetJSONRoot() + "levels.json";
        }

        public string GetJSONSavedLevelPath()
        {
            return GetJSONRoot() + "savedLevel.json";
        }

        public string GetSoundsRoot()
        {
            return "Sounds/";
        }


        public string GetBallSoundsRoot()
        {
            return GetSoundsRoot() + "Ball/";
        }


        public string GetMonstersSoundsRoot()
        {
            return GetSoundsRoot() + "Monsters/";
        }


        public string GetPlayerSoundsRoot()
        {
            return GetSoundsRoot() + "Player/";
        }

        public string GetMusicsSoundsRoot()
        {
            return GetSoundsRoot() + "Musics/";
        }

    }
}
