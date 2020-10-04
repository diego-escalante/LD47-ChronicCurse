using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class KillOnContact : MonoBehaviour {
    
    private BoxCollider2D coll;
    private BoxCollider2D playerColl;
    private PlayerLifeController plc;

    private void Start() {
        coll = GetComponent<BoxCollider2D>();
        playerColl = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        plc = playerColl.GetComponent<PlayerLifeController>();
    }

    private void Update() {
        if (coll.Overlaps(playerColl)) {
            plc.Die();
        }
    }
}
