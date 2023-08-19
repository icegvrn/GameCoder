using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PortalDBContainer))]
[RequireComponent(typeof (BestTimeFromDB))]
public class PortalBehavior : MonoBehaviour
{
    [SerializeField] private bool isAvailableForPlayer;
    public bool IsAvailableForPlayer { get { return isAvailableForPlayer; } set { isAvailableForPlayer = value; } }

    [SerializeField] private Material unavailableMat;
    [SerializeField] private GameObject portalElement;
    [SerializeField] private Material unavailablePortalMat;
    [SerializeField] private Material availablePortalMat;
    [SerializeField] private GameObject ConsultationSpots;

    private List<Material> initialMat;

    public void Init()
    {
        initialMat = new List<Material>();
        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject != portalElement)
            {
                initialMat.Add(renderer.material);
            }
        }

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

        ConsultationSpots.SetActive(true);

        int matIndex = 0;
        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject != portalElement)
            {
                renderer.material = initialMat[matIndex];
                matIndex++;
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
        ConsultationSpots.SetActive(false);
    }
}
