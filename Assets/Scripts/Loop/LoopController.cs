using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopController : MonoBehaviour {

    public float loopDuration = 5f;
    private float loopTimeLeft;

    private List<ILooper> subscribers = new List<ILooper>();

    private void OnEnable() {
        loopTimeLeft = loopDuration;
    }

    private void Update() {
        loopTimeLeft -= Time.deltaTime;
        if (loopTimeLeft <= 0) {
            UpdateLoopers();
            loopTimeLeft = loopDuration;
        }
    }

    public void Subscribe(ILooper iLooper) {
        subscribers.Add(iLooper);
        iLooper.SetState();
    }

    public void Unsubscribe(ILooper iLooper) {
        subscribers.Remove(iLooper);
    }

    private void UpdateLoopers() {
        foreach(ILooper iLooper in subscribers) {
            if (iLooper.IsLooping()) {
                iLooper.Loop();
            } else {
                iLooper.SetState();
            }
        }
    }

}
