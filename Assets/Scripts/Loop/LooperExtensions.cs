using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LooperExtensions {

    static private LoopController loop;
    
    public static void SubscribeToLoop(this ILooper iLooper) {
        LoopController loop = getLoop();
        if (loop == null) {
            Debug.LogError("Could not find LoopController to subscribe to!");
            return;
        }
        loop.Subscribe(iLooper);
    }

    public static void UnsubscribeFromLoop(this ILooper iLooper) {
        LoopController loop = getLoop();
        if (loop == null) {
            return;
        }
        loop.Unsubscribe(iLooper);
    }

    private static LoopController getLoop() {
        if (loop == null) {
            GameObject gb = GameObject.FindGameObjectWithTag("GameController");
            if (gb == null) {
                return null;
            }
            loop = gb.GetComponent<LoopController>();
        }
        return loop;
    }
}
