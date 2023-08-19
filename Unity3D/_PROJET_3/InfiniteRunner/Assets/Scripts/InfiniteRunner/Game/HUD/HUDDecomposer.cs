using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDDecomposer : MonoBehaviour
{
    [SerializeField] List<Image> lifeIcons;
    public List<Image> LifeIcons { get { return lifeIcons; } }
    [SerializeField] TextMeshProUGUI username;
    public TextMeshProUGUI Username { get { return username; } }
    [SerializeField] TextMeshProUGUI timer;
    public TextMeshProUGUI Timer { get {  return timer; } }
    [SerializeField] TextMeshProUGUI essences;
    public TextMeshProUGUI Essences { get { return essences; } }

    [SerializeField] TextAnimator bestTimeAnimator;
    public TextAnimator BestTimeAnimator { get { return bestTimeAnimator; } }

}
