using UnityEngine;

public class SoundLevelService : MonoBehaviour
{
    float musicLevel;
    public float MusicLevel { get { return musicLevel; } set { musicLevel = value; } }
    float vfxLevel;
    public float VFXLevel { get { return vfxLevel; } set { vfxLevel = value; } }


    public SoundLevelService()
    {
        if (ServiceLocator.Instance.GetService<SoundLevelService>() == null)
        {
            ServiceLocator.Instance.RegisterService(this);
        }
    }
}
