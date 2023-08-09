using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof (PortalDBContainer))]
[RequireComponent(typeof (BestTimeFromDB))]
public class PortalBehavior : MonoBehaviour
{
    [SerializeField] private bool isAvailableForPlayer;
    public bool IsAvailableForPlayer { get { return isAvailableForPlayer; } set { isAvailableForPlayer = value; } }
   
 

    [SerializeField] private Material unavailableMat;
    [SerializeField] private Material availableMat;

    [SerializeField] private GameObject portalElement;
    [SerializeField] private Material unavailablePortalMat;
    [SerializeField] private Material availablePortalMat;
    // Start is called before the first frame update

   
    
    public void Init()
    {
        DisablePortal();

        if (isAvailableForPlayer)
        {
            EnablePortal();
        }
    
}

    void EnablePortal()
    {
   
        portalElement.GetComponent<Renderer>().material = availablePortalMat;
        if (portalElement.TryGetComponent(out Teleporter teleporter))
        {
            teleporter.IsEnable = true;
        }

        if (portalElement.TryGetComponent(out SphereCollider sCollider))
        {
            sCollider.radius = 1.5f;
        }

        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject != portalElement)
            {
                renderer.material = availableMat;
            }
        }

        GetComponent<PortalDBContainer>().SpawnBestTimePanel();

    }

    void DisablePortal()
    {
        portalElement.GetComponent<Renderer>().material = unavailablePortalMat;

        if (portalElement.TryGetComponent(out Teleporter teleporter))
        {
            teleporter.IsEnable = false;
        }

        if (portalElement.TryGetComponent(out SphereCollider sCollider))
        {
            sCollider.radius = 15f;
        }


        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject != portalElement)
            {
                renderer.material = unavailableMat;
            }
        }
    }
}
