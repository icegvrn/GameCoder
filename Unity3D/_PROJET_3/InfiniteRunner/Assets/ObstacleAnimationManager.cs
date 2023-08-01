using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAnimationManager : MonoBehaviour
{
    [SerializeField] private List<Animator> animators;
    [SerializeField] private bool CanBePlayedAtTheSameTime;
    [SerializeField] private int LimitAtTheSameTime;
    // Start is called before the first frame update
    void Start()
    {
        // EnableAnimation();
    }

    // Update is called once per frame
    void Update()
        
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Animator animator in animators)
            {   
                    animator.Play("IDLE");
            }
        }
    }

    public void EnableAnimation()
    {
        int nbOfAnimationsToPlay = 1;

        if (CanBePlayedAtTheSameTime)
        {
            int count = animators.Count;
            if (LimitAtTheSameTime > 0)
            {
                count = LimitAtTheSameTime;
            }
            nbOfAnimationsToPlay = Random.Range(0, count);
        }

        for (int i = 0; i < nbOfAnimationsToPlay; i++)
        {

            int indexAnimatorToEnable = Random.Range(0, animators.Count);
            Debug.Log("JE PASSE LA AVEC UN RANDOM DE " + indexAnimatorToEnable);
            Animator animatorToEnable = animators[indexAnimatorToEnable];
            animatorToEnable.speed = 1;
            animatorToEnable.SetBool("animate", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController controller))
        {
            EnableAnimation();
        }
    }

    public void Reset()
    {
        foreach (Animator animator in animators)
        {
                animator.Play("IDLE");
        }
    }
}
