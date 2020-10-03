using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopController : MonoBehaviour {

    public float loopDuration = 5f;
    private float loopTimeLeft;
    private Slider loopSlider;

    private List<ILooper> subscribers = new List<ILooper>();

    private void Awake() {
        loopSlider = GameObject.FindGameObjectWithTag("UI").transform.Find("LoopSlider").GetComponent<Slider>();
    }

    private void OnEnable() {
        loopTimeLeft = loopDuration;
    }

    private void Update() {
        loopTimeLeft -= Time.deltaTime;
        loopSlider.value = GetLoopPercentage();
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

    public float GetLoopPercentage() {
        return Mathf.Clamp((loopDuration-loopTimeLeft)/loopDuration, 0, 1);
    }

}
