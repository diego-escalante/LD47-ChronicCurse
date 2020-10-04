using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour {
    
    public bool canDie = true;
    public bool justRevived = false;

    private float invinsibilityTime = 2f;
    private float timeLeft;
    private PlayerMovement movement;
    private SoundController sound;
    private CamShake cam;
    private SpriteRenderer rend;
    private SpriteColorShifter scs;
    private Animator animator;

    private void Start() {
        movement = GetComponent<PlayerMovement>();
        sound = GameObject.FindWithTag("GameController").GetComponent<SoundController>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CamShake>();
        rend = GetComponent<SpriteRenderer>();
        scs = GetComponent<SpriteColorShifter>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (justRevived) {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0) {
                justRevived = false;
                canDie = true;
                scs.Stop();
                rend.color = Color.white;
            }
        }
    }

    public void Die() {
        if (!canDie) {
            return;
        }
        cam.BigShake();
        sound.playSnapBackSound();
        movement.enabled = false;
        animator.enabled = false;
        transform.eulerAngles = new Vector3(0, 0, 90);
        canDie = false;
    }

    public void Revive() {
        if (canDie) {
            return;
        }
        movement.enabled = true;
        animator.enabled = true;
        transform.eulerAngles = Vector3.zero;
        timeLeft = invinsibilityTime;
        scs.Play();
        justRevived = true;
    }

}
