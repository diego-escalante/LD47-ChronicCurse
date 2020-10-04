using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

    private Image keyIcon;
    private bool hasKey = false;
    private SoundController sound;

    public void Start() {
        sound = GameObject.FindWithTag("GameController").GetComponent<SoundController>();
        keyIcon = GameObject.FindWithTag("UI").transform.Find("Key").GetComponent<Image>();
        keyIcon.enabled = hasKey;
    }

    public bool Haskey() {
        return hasKey;
    }

    public void GetKey() {
        hasKey = true;
        keyIcon.enabled = true;
    }

    public void UseKey() {
        hasKey = false;
        keyIcon.enabled = false;
    }
}
