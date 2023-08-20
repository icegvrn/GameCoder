using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// PortalBehavior choisit le comportement � adopter pour un portail en fonction de s'il est actif ou inactif pour un utilisateur donn� (d�termin� via PortalEnablerFromDB)
/// </summary>
[RequireComponent(typeof(PortalDBContainer))]
[RequireComponent(typeof(BestScoreTimeFromDB))]
public class PortalBehavior : MonoBehaviour
{
    private bool isAvailableForPlayer;
    public bool IsAvailableForPlayer { get { return isAvailableForPlayer; } set { isAvailableForPlayer = value; } }

    [Header("Mat�riaux structure portail")]
    [SerializeField] private Material unavailableMat;
    [SerializeField] private GameObject portalElement;    
    
    [Header("Mat�riaux coeur portail")]
    [SerializeField] private Material unavailablePortalMat;
    [SerializeField] private Material availablePortalMat;

    [Header("Spots de consultation associ�s au portail")]
    [SerializeField] private GameObject ConsultationSpots;

    private List<Material> initialMat;

    public void Init()
    {
        RegisterInitialMaterials();
       
        DisablePortal();

        if (isAvailableForPlayer)
        {
            EnablePortal();
        }
    }

    void RegisterInitialMaterials()
    {
        initialMat = new List<Material>();

        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject != portalElement)
            {
                initialMat.Add(renderer.material);
            }
        }
    }

    void EnablePortal()
    {
        ActivePortalTeleporter(true);
        ConsultationSpots.SetActive(true);
        GetComponent<PortalDBContainer>().SpawnBestTimePanel(); 
        SetPortalMaterials(availablePortalMat, true);
    }

    void DisablePortal()
    {
        
        ActivePortalTeleporter(false);
        ConsultationSpots.SetActive(false);
        SetPortalMaterials(unavailablePortalMat, false);
    }


    /// <summary>
    ///  Modifie les mat�riaux en fonction de l'�tat du portail ; le coeur du portail a un material l�g�rement diff�rent
    /// </summary>
    /// <param name="portalMaterial"></param>
    /// <param name="setInitialMaterial"></param>
    void SetPortalMaterials(Material portalMaterial, bool setInitialMaterial)
    {
        portalElement.GetComponent<Renderer>().material = portalMaterial;

        int matIndex = 0;

        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject != portalElement)
            {
                if (setInitialMaterial)
                {
                    renderer.material = initialMat[matIndex];
                }
                else
                {
                    renderer.material = unavailableMat;
                }
                matIndex++;
            }
        }
    }

    /// <summary>
    /// R�duit ou augmente la port�e du portail selon l'�tat : un petit radius permet de ne pas se t�l�porter par erreur quand le portail est actif, un grand radius permet d'afficher un message de loin quand il est inactif.
    /// </summary>
    /// <param name="b"></param>
    void ActivePortalTeleporter(bool b)
    {
        portalElement.GetComponent<SphereCollider>().radius = b ? 1.5f : 15f;
        portalElement.GetComponent<Teleporter>().IsEnable = b;
    }
}
