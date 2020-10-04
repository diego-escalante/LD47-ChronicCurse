using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CollisionController))]
public class ArrowMovement : MonoBehaviour {
   
    public float speed = 2f;
    private CollisionController collisionController;

    void Start() {
        collisionController = GetComponent<CollisionController>();
    }

    void Update() {
        Vector2 move = Vector2.right * speed * Time.deltaTime;
        Vector2 newMove = collisionController.Check(move).moveVector;
        if (move != newMove) {
            Destroy(gameObject);
        } else {
            transform.Translate(move);
        }
    }

}
