using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopController : MonoBehaviour {

    public float loopDuration = 4f;
    private float prevLoopTime;
    private Slider loopSlider;
    private MusicController musicController;

    private List<ILooper> subscribers = new List<ILooper>();

    private void Awake() {
        Transform UITrans = GameObject.FindGameObjectWithTag("UI").transform;
        loopSlider = UITrans.Find("LoopSlider").GetComponent<Slider>();
        musicController = GetComponent<MusicController>();
        musicController.StartMusic();
        prevLoopTime = loopDuration;
    }

    private void Update() {
        float currentLoopTime = loopDuration - musicController.main.time % loopDuration;
        if (loopSlider == null) {
            loopSlider = GameObject.FindGameObjectWithTag("UI").transform.Find("LoopSlider").GetComponent<Slider>();
        }
        if (loopSlider != null && loopSlider.IsActive()) {
            loopSlider.value = GetLoopPercentage();
        }
        if (currentLoopTime > prevLoopTime) {
            UpdateLoopers();
        }
        prevLoopTime = currentLoopTime;
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
        return Mathf.Clamp((loopDuration-prevLoopTime)/loopDuration, 0, 1);
    }
}
