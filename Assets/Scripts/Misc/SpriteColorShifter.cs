using System.Collections.Generic;
using UnityEngine;

/*
 * The SpriteColorShifter can be used to do simple basic color transitions for a sprite by
 * setting transitions in the form of Colors and time to transition into that color.
 * The color shifter can play, stop, loop, and step through all transitions. The shifter
 * can be useful for things like indicating invinsiblity frames or when damage is dealt.
 * You can modify transitions programmatically by getting the transitions list, modifying
 * it, and setting it back.
 */

public class SpriteColorShifter : MonoBehaviour {

    [SerializeField]
    private bool playing = true;
    [SerializeField]
    private bool looping = true;
    [SerializeField]
    private List<ColorTransition> colorTransitions = new List<ColorTransition>();

    private SpriteRenderer spriteRenderer;
    private float transitionTimeLeft;
    private Color currentColor;
    private int currentTransition = 0;
    private bool stepping = false;

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogError(string.Format("There is no SpriteRenderer attached to game object {0} for the SpriteColorShifter to work.", gameObject.name));
            this.enabled = false;
        }
        if (colorTransitions.Count < 1) {
            Debug.LogError(string.Format("The SpriteColorShifter in game object {0} has no transitions, rendering it useless.", gameObject.name));
            this.enabled = false;
        }
        currentColor = spriteRenderer.color;
        transitionTimeLeft = colorTransitions[0].timeInSecs;
	}
	
	void Update () {
        // Don't do anything if the shifter is not playing.
        if (!playing && !stepping) {
            return;
        }
        
        // Update sprite color.
        transitionTimeLeft -= Time.deltaTime;
        spriteRenderer.color = Color.Lerp(currentColor, colorTransitions[currentTransition].color, 1 - (transitionTimeLeft / colorTransitions[currentTransition].timeInSecs));

        // If current transition is over.
        if (transitionTimeLeft <= 0) {
            // Change transition.
            currentColor = spriteRenderer.color;
            currentTransition = (currentTransition + 1) % colorTransitions.Count;
            transitionTimeLeft = colorTransitions[currentTransition].timeInSecs;

            // If we were stepping, stop stepping.
            stepping = false;

            // If not looping and reached back to the beginning of the transitions, stop playing.
            if (!looping && currentTransition == 0) {
                playing = false;
                return;
            }
        }
	}

    public void Play() {
        playing = true;
    }

    public void Stop() {
        playing = false;
    }

    public void Step() {
        stepping = true;
    }

    public void SetLooping(bool looping) {
        this.looping = looping;
    }

    public void SetCurrentTransition(int i) {
        if (colorTransitions.Count == 0) {
            Debug.LogError(string.Format("There are no transitions in SpriteColorShifter for game object {0}!", gameObject.name));
            return;
        }
        if (i >= colorTransitions.Count) {
            Debug.LogError(string.Format("There is no transition {0} in SpriteColorShifter for game object {1}. Possible range is [0, {2}].", i, gameObject.name, colorTransitions.Count - 1));
            return;
        }
        currentColor = spriteRenderer.color;
        currentTransition = i;
        transitionTimeLeft = colorTransitions[currentTransition].timeInSecs;
    }

    public void setColorTransitions(List<ColorTransition> colorTransitions) {
        if (colorTransitions == null || colorTransitions.Count == 0) {
            Debug.LogError(string.Format("Cannot set colorTransitions for SpriteColorShifter for game object {0}, as it is either null or empty.", gameObject.name));
        }
        this.colorTransitions = colorTransitions;
        SetCurrentTransition(0);
    }

    public List<ColorTransition> GetColorTransitions() {
        return colorTransitions;
    }

    [System.Serializable]
    public struct ColorTransition {
        public float timeInSecs;
        public Color color;

        ColorTransition(Color color, float timeInSecs) {
            this.color = color;
            this.timeInSecs = timeInSecs;
        }
    }
}
