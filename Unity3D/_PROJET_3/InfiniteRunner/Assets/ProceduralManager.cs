using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralManager : MonoBehaviour
{
    public oldPathGenerator pathGenerator;
    public DecorGenerator decoratorGenerator;
    // Start is called before the first frame update
    void Start()
    {
        pathGenerator.ProceduralStart();
    }

    // Update is called once per frame
    void Update()
    {
        pathGenerator.ProceduralUpdate();
        
    }
    private void LateUpdate()
    {
    }
}
