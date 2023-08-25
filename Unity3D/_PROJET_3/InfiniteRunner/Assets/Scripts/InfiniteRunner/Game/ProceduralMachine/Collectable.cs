using System.Collections;
using UnityEngine;

/// <summary>
/// Définit un objet comme étant collectable : il ajoute un nombre de "points" défini lorsqu'il est trigger par un objet ayant un "pointCollector".
/// </summary>
public class Collectable : MonoBehaviour
{
    [SerializeField] private int pointsValue;
    [SerializeField] private AudioSource audioSource;
    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        if (!collected && other.gameObject.TryGetComponent(out PointCollector pCollector))
        {
           
            pCollector.AddPoints(pointsValue);
            IsVisible(false);
            audioSource.Play();
            collected = true; 
            StartCoroutine(DeactivateAfterDelay(0.1f));
        }
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        IsVisible(true);
        collected = false;

    }

    void IsVisible(bool visibility)
    {
        GetComponent<MeshRenderer>().enabled = visibility;
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = visibility;
        }
    }
}
