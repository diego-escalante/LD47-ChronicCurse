using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class EnableLoopTrigger : MonoBehaviour {
    
    private BoxCollider2D playerColl, coll;
    
    void Start() {
        coll = GetComponent<BoxCollider2D>();
        playerColl = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
    }

    void Update() {
        if (coll.Overlaps(playerColl)) {
            // GameObject.FindGameObjectWithTag("GameController").GetComponent<LoopController>().enabled = true;
            playerColl.GetComponent<PlayerLooper>().enabled = true;
            this.enabled = false;
        }
    }
}
