using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

    private Image image;
    private Coroutine co;

    private void Awake() {
        image = GameObject.FindWithTag("UI").transform.Find("Fader").GetComponent<Image>();
    }

    public void Fade(Color startColor, Color endColor, float duration) {
        if (co != null) {
            StopCoroutine(co);
        }
        co = StartCoroutine(FadeCoroutine(startColor, endColor, duration));
    }

    private IEnumerator FadeCoroutine(Color startColor, Color endColor, float duration) {
        float elapsedTime = 0;
        while (elapsedTime <= duration) {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, elapsedTime/duration);
            yield return null;
        }
    }

}
