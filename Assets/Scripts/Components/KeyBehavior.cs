using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour {
    
    private bool keyExists = true;
    private PlayerInventory pInv;
    private BoxCollider2D coll;
    private BoxCollider2D playerColl;
    private SpriteRenderer rend;

    private void Start() {
        coll = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
        playerColl = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        pInv = playerColl.GetComponent<PlayerInventory>();
    }

    void Update() {
        if (coll.Overlaps(playerColl) && !pInv.Haskey() && keyExists) {
            pInv.GetKey();
            keyExists = false;
            rend.enabled = false;
        }
    }

    public void ResetKey() {
        keyExists = true;
        rend.enabled = true;
    }
}
