using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooper : MonoBehaviour, ILooper {
    
    private bool isLooping = true;
    private Vector2 position;

    private void OnEnable() {
        this.SubscribeToLoop();
    }

    private void OnDisable() {
        this.UnsubscribeFromLoop();
    }

    public bool IsLooping() {
        return isLooping;
    }

    public void SetState() {
        position = transform.position;
    }

    public void Loop() {
        transform.position = position;
    }

}
