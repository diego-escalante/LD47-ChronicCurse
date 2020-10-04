using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserBehavior : MonoBehaviour {
    
    public GameObject arrowPrefab;
    public float interval = 2f;
    public float timeLeft = 0;

    private void Update() {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) {
            timeLeft = interval;
            Instantiate(arrowPrefab, transform.position, transform.localRotation);
        }
    }

}
