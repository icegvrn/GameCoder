using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet de gérer l'animation d'un obstacle : lit l'animation quand la sphère de collision trigger avec un joueur.
/// </summary>
public class ObstacleAnimationManager : MonoBehaviour
{   
    
    [Header("Configuration de l'animation")]
    [SerializeField] private List<Animator> animators;

    [Header("Si plusieurs animators")]
    [SerializeField] private bool CanBePlayedAtTheSameTime;
    [SerializeField] private int LimitAtTheSameTime;

    // Constante définissant le nom des animations à jouer
    private const string IDLE_STATE = "IDLE";
    private const string ANIMATION_STATE = "animate";

    /// <summary>
    /// Appelle la méthode d'animation quand le collider est trigger par un joueur.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterAutoRunner controller))
        {
            EnableAnimation();
        }
    }

    public void EnableAnimation()
    {
        int nbOfAnimationsCanBePlayed = CaculateNbAnimationToPlay();

        for (int i = 0; i < nbOfAnimationsCanBePlayed; i++)
        {
            PlayRandomAnimation();
        }
    }

    /// <summary>
    /// Calcul le nombre d'animation qu'il peut jouer en tenant compte du paramétrage booléen permettant de les jouer en même temps ou non.
    /// </summary>
    /// <returns></returns>
    int CaculateNbAnimationToPlay()
    {
        int nb = 1;

        if (CanBePlayedAtTheSameTime)
        {
            int count = animators.Count;

            if (LimitAtTheSameTime > 0)
            {
                count = LimitAtTheSameTime;
            }
            nb = Random.Range(0, count);
        }

        return nb;
    }

    /// <summary>
    /// Joue une animation parmis toutes celle disponibles
    /// </summary>
    void PlayRandomAnimation()
    {
        int indexAnimatorToEnable = Random.Range(0, animators.Count);
        Animator animatorToEnable = animators[indexAnimatorToEnable];
        animatorToEnable.speed = 1;
        animatorToEnable.SetBool(ANIMATION_STATE, true);
    }

    /// <summary>
    /// Reset en remettant l'animation sur IDLE
    /// </summary>
    public void Reset()
    {
        foreach (Animator animator in animators)
        {
                animator.Play(IDLE_STATE);
        }
    }
}
