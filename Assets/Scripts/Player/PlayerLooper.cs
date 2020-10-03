using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooper : MonoBehaviour, ILooper {
    
    public int maxBreakCharges = 1;
    public int loopsToChargeBreak = 3;
    
    private bool isLooping = true;
    private int loopsToChargeBreakLeft;
    private Vector2 position;
    private int breakCharges = 0;

    private void OnEnable() {
        this.SubscribeToLoop();
        loopsToChargeBreakLeft = loopsToChargeBreak;
    }

    private void OnDisable() {
        this.UnsubscribeFromLoop();
    }

    private void Update() {
        if (Input.GetButton("Action") && breakCharges > 0) {
            BreakLoop();
        }
    }

    public bool IsLooping() {
        return isLooping;
    }

    public void SetState() {
        position = transform.position;
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

}
