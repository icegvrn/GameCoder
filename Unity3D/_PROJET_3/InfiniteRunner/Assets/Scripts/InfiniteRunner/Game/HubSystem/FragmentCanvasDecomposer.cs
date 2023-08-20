using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FragmentCanvasDecomposer : MonoBehaviour
{
    [SerializeField] Image img;
    public Image Img { get { return img; } set { img = value; } }

    [SerializeField] TextMeshProUGUI title;
    public TextMeshProUGUI Title { get { return title; } set { title = value; } }
    [SerializeField] TextMeshProUGUI time;
    public TextMeshProUGUI Time { get { return time; } set { time = value; } }
    [SerializeField] TextMeshProUGUI content;
    public TextMeshProUGUI Content { get { return content; } set { content = value; } }
    [SerializeField] TextMeshProUGUI textOverOrangeInformation;
    public TextMeshProUGUI TextOverOrangeInformation { get { return textOverOrangeInformation; } set { textOverOrangeInformation = value; } }
    [SerializeField] TextMeshProUGUI orangeInformation;
    public TextMeshProUGUI OrangeInformation { get { return orangeInformation; } set { orangeInformation = value; } }

    [SerializeField] List<Sprite> imgSprites;
    public List<Sprite> ImgSprite { get { return imgSprites; } set { imgSprites = value; } }

}
