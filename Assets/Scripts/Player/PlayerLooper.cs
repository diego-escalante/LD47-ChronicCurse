using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class PlayerLooper : MonoBehaviour, ILooper {
    
    public int maxBreakCharges = 1;
    public int loopsToChargeBreak = 3;
    
    private bool isLooping = true;
    private int loopsToChargeBreakLeft;
    private Vector2 position;
    private int breakCharges = 0;

    private SpriteRenderer loopGhost;
    private SpriteRenderer spriteRenderer;

    private void OnEnable() {
        this.SubscribeToLoop();
        loopsToChargeBreakLeft = loopsToChargeBreak;
    }

    private void OnDisable() {
        this.UnsubscribeFromLoop();
    }

    public void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        if (Input.GetButton("Action") && breakCharges > 0) {
            BreakLoop();
        }
    }

    public bool IsLooping() {
        return isLooping;
    }

    public void SetState() {
        position = transform.position;
        createLoopGhost();
        isLooping = true;
    }

    public void Loop() {
        transform.position = position;
        if (breakCharges < maxBreakCharges) {
            loopsToChargeBreakLeft--;
            if (loopsToChargeBreakLeft <= 0) {
                breakCharges++;
                loopsToChargeBreakLeft = loopsToChargeBreak;
            }
        }
    }

    private void BreakLoop() {
        breakCharges--;
        isLooping = false;
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

        loopGhost.sprite = spriteRenderer.sprite;
        loopGhost.transform.position = transform.position;
    }
}
