using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// G�re un groupe de collectables (objet que le joueur peut collecter dans le jeu) en contenant une liste de collectable qu'il peut masquer ou afficher.
/// </summary>
public class CollectablesGroup : MonoBehaviour
{
    [SerializeField] private List<Collectable> collectables;
    public List<Collectable> Collectables { get { return collectables; } }

    /// <summary>
    /// A l'initiation, ajoute tous les enfants "collectable" � sa liste pour ne pas avoir � le faire manuellement. 
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
    /// Le reset d'un collectable group est appel� lorsqu'il va �tre r�utilis� par la pool. Il consiste donc � r�afficher les collectables. 
    /// </summary>
    public void Reset()
    {
        EnableGroup();
    }
}
