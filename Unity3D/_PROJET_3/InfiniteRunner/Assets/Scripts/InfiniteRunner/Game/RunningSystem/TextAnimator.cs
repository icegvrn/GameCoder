using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Permet de donner un effet d'animation à une succesion de texte
/// </summary>
public class TextAnimator : MonoBehaviour
{
    [Header("Element texte")]
    [SerializeField] private TextMeshProUGUI textElem;

    [Header("Configuration de l'animation")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float maxScale = 2.0f;
    [SerializeField] private float lastTextPersistenceLenght;

    [Header("Contenu du texte")]
    [SerializeField] private string[] textsToAnimate = { "Ready?", "5", "4", "3", "2", "1", "Go!" };
    public string[] TextsToAnimate { get { return textsToAnimate; } set { textsToAnimate = value; } }

    [Header("Actions d'animation")]
    [SerializeField] private UnityEvent OnAnimationStart;
    [SerializeField] private UnityEvent OnLastText;
    [SerializeField] private UnityEvent OnAnimationEnd;

    /// <summary>
    /// Démarre une coroutine sur un OnEnable pour pouvoir être rappelé plusieurs fois tout en gardant l'animation
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine(PlayCountdown());
    }



    private IEnumerator PlayCountdown()
    {
        // Une action pour chaque texte à animer
        for (int i = 0; i < TextsToAnimate.Length; i++)
        {
            textElem.text = TextsToAnimate[i];

            // Initialise le texte en transparent
            textElem.color = new Color(textElem.color.r, textElem.color.g, textElem.color.b, 0f);

            // Initialise le texte à une taille plus grande que sa taille voulue pour un effet "dézoom"
            textElem.transform.localScale = Vector3.one * maxScale;

            // Initialisation d'un timer 
            float elapsedTime = 0f;

          
            while (elapsedTime < fadeDuration)
            {

                float t = elapsedTime / fadeDuration;

                // Lerp qui modifie la transparence du texte pour aller progressivement vers 1f
                float fadeAlpha = Mathf.Lerp(0f, 1f, t);
                textElem.color = new Color(textElem.color.r, textElem.color.g, textElem.color.b, fadeAlpha);

                // Lerp qui modifi la taille du texte pour aller progressivement vers 1f
                float scale = Mathf.Lerp(maxScale, 1f, t);
                textElem.transform.localScale = Vector3.one * scale;

                // Incrémente le timer
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            // Met le texte à sa configuration finale
            textElem.color = new Color(textElem.color.r, textElem.color.g, textElem.color.b, 1f);
            textElem.transform.localScale = Vector3.one;

            if (i == 0)
            {
                OnAnimationStart.Invoke();
            }

            yield return new WaitForSeconds(0.5f); // Effectue une pause avant d'afficher le texte suivant

           
            // Si c'était le dernier texte, on attend le temps du "lastTextPersistenceLenght", utilisé pour faire "trainer" certains infos plus longtemps
             if (i == TextsToAnimate.Length - 1 && lastTextPersistenceLenght != 0)
            {
                OnLastText.Invoke();
                yield return new WaitForSeconds(lastTextPersistenceLenght);
                textElem.text = ""; // Supprime le texte
                OnAnimationEnd.Invoke(); // Invoque l'action à effectuer par la suite
            }
        }
    }
}


