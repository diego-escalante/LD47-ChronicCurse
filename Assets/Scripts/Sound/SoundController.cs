using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioClip breakSound;
    public AudioClip jumpSound;
    public AudioClip setStateSound;
    public AudioClip snapBackSound;

    public AudioSource audioSource;

    public void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void playBreakSound() {
        audioSource.PlayOneShot(breakSound, 1);
    }

    public void playJumpSound() {
        audioSource.PlayOneShot(jumpSound, 1);
    }

    public void playSnapBackSound() {
        audioSource.PlayOneShot(snapBackSound, 1);
    }

    public void playSetStateSound() {
        audioSource.PlayOneShot(setStateSound, 1);
    }
}