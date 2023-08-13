using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextAnimator : MonoBehaviour
{

    public TextMeshProUGUI textElem;
    public float fadeDuration = 0.5f;
    public float maxScale = 2.0f;
    public string[] textsToAnimate = { "Ready?", "5", "4", "3", "2", "1", "Go!" };
    public float lastTextPersistenceLenght;

    [SerializeField] UnityEvent OnAnimationEnd;
    private void OnEnable()
    {
        StartCoroutine(PlayCountdown());
    }

    private IEnumerator PlayCountdown()
    {
        for (int i = 0; i < textsToAnimate.Length; i++)
        {
            textElem.text = textsToAnimate[i];
            textElem.color = new Color(textElem.color.r, textElem.color.g, textElem.color.b, 0f); // Initial transparency
            textElem.transform.localScale = Vector3.one * maxScale; // Initial scale

            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                float t = elapsedTime / fadeDuration;
                float fadeAlpha = Mathf.Lerp(0f, 1f, t);
                textElem.color = new Color(textElem.color.r, textElem.color.g, textElem.color.b, fadeAlpha);

                float scale = Mathf.Lerp(maxScale, 1f, t);
                textElem.transform.localScale = Vector3.one * scale;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textElem.color = new Color(textElem.color.r, textElem.color.g, textElem.color.b, 1f);
            textElem.transform.localScale = Vector3.one;

            yield return new WaitForSeconds(0.5f); // Pause before next countdown

      
     
                if (i == textsToAnimate.Length - 1 && lastTextPersistenceLenght != 0)
                {
                yield return new WaitForSeconds(lastTextPersistenceLenght); 
                   textElem.text = "";
                OnAnimationEnd.Invoke();
            }

            
            
          
        }
    }
}


