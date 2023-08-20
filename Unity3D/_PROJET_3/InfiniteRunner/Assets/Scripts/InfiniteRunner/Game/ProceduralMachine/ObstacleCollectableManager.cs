using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// G�re les groupes de collectables pr�sent sur un obstacle : permet de choisir un � afficher au hasard, de masquer les autres en communiquant avec leur collectablesGroup.
/// </summary>
[RequireComponent(typeof(Obstacle))]
public class ObstacleCollectableManager : MonoBehaviour
{
   // Liste de groupes de collectables
   private List<CollectablesGroup> collectablesGroups;

    void Start()
    {
        InitializeManager();
        InitializeAllCollectablesGroups();
        EnableARandomCollectablesGroup();
    }

    void InitializeManager()
    {
        collectablesGroups = new List<CollectablesGroup>();
    }

    void InitializeAllCollectablesGroups()
    {
        foreach (CollectablesGroup collectablesGroup in GetComponentsInChildren<CollectablesGroup>())
        {
            collectablesGroups.Add(collectablesGroup);
            collectablesGroup.Init();
        }
    }

    /// <summary>
    /// M�thode permettant d'activer un groupe de collectable au hasard en d�sactivant tout puis en r�activant l'un au hasard.
    /// </summary>
    public void EnableARandomCollectablesGroup()
    {
        ResetAllCollectablesGroup();
        ChooseAndActiveRandomGroup();      
    }

    /// <summary>
    /// Reset permettant aux �ventuels collectables d�sactiv�s par le passage d'un joueur d'�tre r�activ�s pour �tre r�utilis� (n�cessaire avec la pool)
    /// </summary>
    void ResetAllCollectablesGroup()
    {
        foreach (CollectablesGroup collectablesGroup in collectablesGroups)
        {
            collectablesGroup.Reset();
        }
    }

    /// <summary>
    /// En tirant un random parmis tous les groupes, active le groupe choisi. Utilise un random de 1 plus grand pour permettre l'affichage d'aucun groupe.
    /// </summary>
    void ChooseAndActiveRandomGroup()
    {
        int randNb = Random.Range(0, collectablesGroups.Count + 1);

        foreach (CollectablesGroup collecContainer in collectablesGroups)
        {
            if (randNb > collectablesGroups.Count || collectablesGroups.IndexOf(collecContainer) != randNb)
            {
                collecContainer.DisableGroup();
            }

            else
            {
                collecContainer.EnableGroup();
            }
        }
    }

    /// <summary>
    /// Le reset permet de choisir un nouveau groupe � afficher.
    /// </summary>
    public void Reset()
    {
        EnableARandomCollectablesGroup();
    }
}
