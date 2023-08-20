using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère les groupes de collectables présent sur un obstacle : permet de choisir un à afficher au hasard, de masquer les autres en communiquant avec leur collectablesGroup.
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
    /// Méthode permettant d'activer un groupe de collectable au hasard en désactivant tout puis en réactivant l'un au hasard.
    /// </summary>
    public void EnableARandomCollectablesGroup()
    {
        ResetAllCollectablesGroup();
        ChooseAndActiveRandomGroup();      
    }

    /// <summary>
    /// Reset permettant aux éventuels collectables désactivés par le passage d'un joueur d'être réactivés pour être réutilisé (nécessaire avec la pool)
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
    /// Le reset permet de choisir un nouveau groupe à afficher.
    /// </summary>
    public void Reset()
    {
        EnableARandomCollectablesGroup();
    }
}
