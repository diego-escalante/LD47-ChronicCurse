using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class DialogueTrigger : MonoBehaviour {
    
    public Dialogue dialogue;
    private BoxCollider2D playerColl;
    private BoxCollider2D coll;

    private void Start() {
        playerColl = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (coll.Overlaps(playerColl)) {
            GameObject.FindWithTag("GameController").GetComponent<DialogueManager>().StartDialogue(dialogue);
            this.enabled = false;
        }
    }
}
