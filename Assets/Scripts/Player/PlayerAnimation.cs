using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Animator), typeof (PlayerMovement), typeof(SpriteRenderer))]
[RequireComponent (typeof (PlayerLooper))]
public class PlayerAnimation : MonoBehaviour {

    private Animator animator;
    private PlayerMovement movement;
    private SpriteRenderer rend;
    private PlayerLooper looper;
    
    private bool isLooping;

    private void Start() {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rend = GetComponent<SpriteRenderer>();
        looper = GetComponent<PlayerLooper>();
        if (looper.enabled) {
            animator.SetTrigger("LoopingTrigger");
        } else {
            animator.SetTrigger("BreakingTrigger");
        }
    }

    private void Update() {
        animator.SetBool("IsGrounded", movement.IsGrounded());
        Vector2 vel = movement.GetVelocity();
        animator.SetFloat("Speed", Mathf.Abs(vel.x));
        if (vel.x != 0) {
            rend.flipX = vel.x > 0 ? false : true;
        }

        if(looper.enabled){
            bool currentIsLooping = looper.IsLooping();
            if (currentIsLooping != isLooping) {
                isLooping = currentIsLooping;
                if (isLooping) {
                    animator.SetTrigger("LoopingTrigger");
                } else {
                    animator.SetTrigger("BreakingTrigger");
                }
            }
        }
    }

}
