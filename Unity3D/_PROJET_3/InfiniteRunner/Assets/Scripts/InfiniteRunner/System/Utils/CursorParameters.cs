using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorParameters : MonoBehaviour
{
    [SerializeField] bool visibility;

    void Start()
    {
        Cursor.visible = visibility;
    }

}
