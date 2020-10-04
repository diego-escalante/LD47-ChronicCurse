using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLooper : MonoBehaviour, ILooper {

    public Sprite loopSwitch;
    public Sprite loopSwitchPressed;
    public Sprite doorLooping;

    private SwitchBehavior switchBehavior;

    public void OnEnable() {
        this.SubscribeToLoop();
        
    }

    public void OnDisable() {
        this.UnsubscribeFromLoop();
    }

    public void Start() {
        SwapSprites();
    }

    public void SetState() {

    }

    public void Loop() {
        GetSwitchBehavior().Unpress();
    }

    public bool IsLooping() {
        return true;
    }

    private SwitchBehavior GetSwitchBehavior() {
        if (switchBehavior == null) {
            switchBehavior = GetComponent<SwitchBehavior>();
        }
        return switchBehavior;
    }

    private void SwapSprites() {
        SwitchBehavior s = GetSwitchBehavior();
        s.sSwitch = loopSwitch;
        s.sSwitchPressed = loopSwitchPressed;
        s.GetComponent<SpriteRenderer>().sprite = s.isPressed ? loopSwitchPressed : loopSwitch;
        s.door.GetComponent<SpriteRenderer>().sprite = doorLooping;
    }
}
