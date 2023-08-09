using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
