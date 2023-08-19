using UnityEngine;

public class SoundLevelService : ISoundService
{
    float musicLevel;
    public float MusicLevel { get { return musicLevel; } set { musicLevel = value; } }
    float vfxLevel;
    public float VFXLevel { get { return vfxLevel; } set { vfxLevel = value; } }

}
