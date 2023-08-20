using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestimeCanvasDecomposer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeName;
    public TextMeshProUGUI TimeName { get { return timeName; } set { timeName = value;  } }

    [SerializeField] TextMeshProUGUI bestTime;
    public TextMeshProUGUI BestTime { get { return bestTime; } set { bestTime = value; } }

    [SerializeField] TextMeshProUGUI fragmentsNb;
    public TextMeshProUGUI FragmentsNb { get { return fragmentsNb; } set { fragmentsNb = value; } }

    [SerializeField] TextMeshProUGUI totalFragmentsFoundInTime;
    public TextMeshProUGUI TotalFragmentsFoundInTime { get { return totalFragmentsFoundInTime; } set { totalFragmentsFoundInTime = value; } }

    [SerializeField] Image image;
    public Image Image { get { return image; } set { image = value; } }

    [SerializeField] List<Sprite> imageSprites;
    public List<Sprite> ImagesSprite { get { return imageSprites; } }
}
