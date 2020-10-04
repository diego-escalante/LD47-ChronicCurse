using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorBehavior : MonoBehaviour {

    private PlayerInventory pInv;
    private BoxCollider2D coll;
    private BoxCollider2D playerColl;

    private void Start() {
        coll = GetComponent<BoxCollider2D>();
        playerColl = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        pInv = playerColl.GetComponent<PlayerInventory>();
    }

    private void Update() {
        if (coll.Overlaps(playerColl) && pInv.Haskey()) {
            pInv.UseKey();
            gameObject.SetActive(false);
        }
    }

}
