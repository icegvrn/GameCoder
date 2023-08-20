using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère un groupe de collectables (objet que le joueur peut collecter dans le jeu) en contenant une liste de collectable qu'il peut masquer ou afficher.
/// </summary>
public class CollectablesGroup : MonoBehaviour
{
    [SerializeField] private List<Collectable> collectables;
    public List<Collectable> Collectables { get { return collectables; } }

    /// <summary>
    /// A l'initiation, ajoute tous les enfants "collectable" à sa liste pour ne pas avoir à le faire manuellement. 
    /// </summary>
    public void Init()
    {
        foreach (Collectable collectable in GetComponentsInChildren<Collectable>())
        {
            collectables.Add(collectable);
        }
    }

    public void EnableGroup()
    {
        foreach (Collectable c in collectables)
        {
            c.gameObject.SetActive(true);
        }
    }

    public void DisableGroup()
    {
      
        foreach (Collectable c in collectables)
        {
            c.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Le reset d'un collectable group est appelé lorsqu'il va être réutilisé par la pool. Il consiste donc à réafficher les collectables. 
    /// </summary>
    public void Reset()
    {
        EnableGroup();
    }
}
