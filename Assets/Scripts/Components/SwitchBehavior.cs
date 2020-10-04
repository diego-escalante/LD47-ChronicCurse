using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour {
    
    public GameObject door;

    public Sprite sSwitch;
    public Sprite sSwitchPressed;
    // public Sprite sSwitchLooped;
    // public Sprite sSwitchLoopedPressed;

    public bool isPressed = false;
    private BoxCollider2D coll;
    private BoxCollider2D playerColl;
    private SpriteRenderer rend;
    private SoundController sound;

    private void Start() {
        coll = GetComponent<BoxCollider2D>();
        playerColl = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
        sound = GameObject.FindWithTag("GameController").GetComponent<SoundController>();
    }

    private void Update() {
        if (!isPressed) {
            if (coll.Overlaps(playerColl)) {
                isPressed = true;
                door.SetActive(false);
                rend.sprite = sSwitchPressed;
                sound.playSetStateSound();
            }
        }
    }

    public void Unpress() {
        if (isPressed) {
            isPressed = false;
            door.SetActive(true);
            rend.sprite = sSwitch;
        }
    }
    

}
