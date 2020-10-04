using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLooper : MonoBehaviour, ILooper {

    public Sprite keyLooper;
    private KeyBehavior keyBehavior;
    private SpriteRenderer loopGhost;
    private SpriteRenderer rend;
    
    private void OnEnable() {
        rend.sprite = keyLooper;
        this.SubscribeToLoop();
    }

    private void OnDisable() {
        this.UnsubscribeFromLoop();
    }

    public bool IsLooping() {
        return true;
    }

    public void Awake() {
        rend = GetComponent<SpriteRenderer>();
        keyBehavior = GetComponent<KeyBehavior>();
    }

    public void SetState() {
        createLoopGhost();
    }

    public void Loop() {
        keyBehavior.ResetKey();
    }

    private void createLoopGhost() {
        // Create ghost if it doesn't exist.
        if (loopGhost == null) {
            GameObject ghostGO = new GameObject(gameObject.name + " Loop Ghost");
            loopGhost = ghostGO.AddComponent<SpriteRenderer>();
            Color c = loopGhost.color;
            c.a = 0.5f;
            loopGhost.color = c;
        }
        loopGhost.sprite = rend.sprite;
        loopGhost.transform.position = transform.position;
    }

}
