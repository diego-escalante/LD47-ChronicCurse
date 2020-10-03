using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LooperExtensions {

    static private LoopController loop;
    
    public static void SubscribeToLoop(this ILooper iLooper) {
        getLoop().Subscribe(iLooper);
    }

    public static void UnsubscribeFromLoop(this ILooper iLooper) {
        getLoop().Unsubscribe(iLooper);
    }

    private static LoopController getLoop() {
        if (loop == null) {
            loop = GameObject.FindGameObjectWithTag("GameController").GetComponent<LoopController>();
        }
        return loop;
    }
}
