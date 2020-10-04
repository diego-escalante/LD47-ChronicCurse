using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

    public AudioSource main;
    public AudioSource justBoops;

    public float volume = 0.5f;

    private void Start() {
        MusicMain();
    }

    public void StartMusic() {
        if (main.isActiveAndEnabled) {
            main.Play();
        }
        if (justBoops.isActiveAndEnabled) {
            justBoops.Play();
        }
    }

    private IEnumerator fadeIn(AudioSource audio, float toVolume, float duration=0.15f) {
        float timeLeft = duration;
        float startingVolume = audio.volume;

        while (timeLeft > 0) {
            timeLeft -= Time.unscaledDeltaTime;
            audio.volume = Mathf.Lerp(startingVolume, toVolume, 1f - (timeLeft/duration));
            yield return null;
        }
        audio.volume = toVolume;
    }

    public void MusicMain() {
        StartCoroutine(fadeIn(main, volume));
        StartCoroutine(fadeIn(justBoops, 0));
    }

    public void MusicBoops() {
        StartCoroutine(fadeIn(justBoops, volume));
        StartCoroutine(fadeIn(main, 0));
    }
}