using TMPro;
using UnityEngine;

public class ClassementItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI username;
    public TextMeshProUGUI Name { get { return username; } set { username = value; } }
    [SerializeField] TextMeshProUGUI fragments;
    public TextMeshProUGUI Fragments { get { return fragments; } set { fragments = value; } }
    [SerializeField] TextMeshProUGUI rank;
    public TextMeshProUGUI Rank { get { return rank; } set { rank = value; } }

}
