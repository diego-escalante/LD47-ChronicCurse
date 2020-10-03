using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CollisionController))]
public class PlatformController : MonoBehaviour {

    public LayerMask solidMask;
    public LayerMask moveableMask;
    public List<Vector2> waypoints;
    public bool isCyclical = false;

    public Vector2 move;
    CollisionController collisionController;

    private const float SKIN = 0.015f;
    private List<Vector2> globalWaypoints = new List<Vector2>();

    void OnValidate() {
        // Ensure there's at least one waypoint, otherwise a moving platform is useless.
        if (waypoints.Count <= 0) {
            waypoints.Add(Vector2.up);
        }
        if (isCyclical && waypoints.Count == 1) {
            isCyclical = false;
        }
    }

    void Start() {
        // Add the platform's position as the first waypoint and convert the local waypoints to global.
        globalWaypoints.Add(transform.position);
        foreach (Vector2 waypoint in waypoints) {
            globalWaypoints.Add(waypoint - (Vector2)transform.position);
        }

        collisionController = GetComponent<CollisionController>();
        if (moveableMask.value == 0) {
            Debug.LogWarning("Platform's movableMask has no layers!");
        } else if (moveableMask.Contains(gameObject.layer) || solidMask.Contains(gameObject.layer)) {
            Debug.LogWarning("One of the LayerMasks contains this object's layer!");
        }
        if (moveableMask.Overlaps(solidMask)) {
            Debug.LogWarning("SolidMask and MoveableMask have at least one layer in common, and they probably should have none.");
        }
    }

    void Update() {
        Vector2 moveVector;
        Vector2 velocity = move * Time.deltaTime;

        // Normal collision checking based on platform's movement, If we even care about collisions for movement.
        if (solidMask.value != 0) {
            moveVector = collisionController.Check(velocity, solidMask).moveVector;
        } else {
            moveVector = velocity;
        }

        // Push passengers and movables on the way.
        // NB: This is done with Translate, rather than using moveable's own collision detection + move.
        // This is because at this point, the design decision is to just not place platforms in a situation where an object might get "squished".

        // Keep track of what has been moved and don't move it twice.
        HashSet<Collider2D> movedPassengers = new HashSet<Collider2D>();

        // Push passengers (movables that are "riding" a platform. Includes players Wall Sticking to the platform.)
        // First case: passengers riding on top. Cast rays and find them, move them.
        List<CollisionController.CollisionInfo> passengersInfos = collisionController.CheckAll(Vector2.up * SKIN, moveableMask);
        foreach (CollisionController.CollisionInfo passengerInfo in passengersInfos) {
            passengerInfo.colliderVertical.transform.Translate(moveVector);
            movedPassengers.Add(passengerInfo.colliderVertical);
        }
        // Second case: PLAYER passengers wall sticking on sides. This is a special snowflake case and I hate having that logic here.
        // Cast rays on both sides, and move everything that is a player and Sticking. Note that only Players can stick (right now).
        // Yeah this all needs a big refactor.
        passengersInfos = collisionController.CheckAll(Vector2.right * SKIN, moveableMask);
        passengersInfos.AddRange(collisionController.CheckAll(Vector2.left * SKIN, moveableMask));
        foreach (CollisionController.CollisionInfo passengerInfo in passengersInfos) {
            // TODO: Take a note out of Seb's Platformer Tutorial EP7 and use a dictionary to reduce GetComponents in the update loop.
            PlayerMovement playerMovement = passengerInfo.colliderHorizontal.GetComponent<PlayerMovement>();
            if (playerMovement != null && playerMovement.IsWallSticking()) {
                playerMovement.transform.Translate(moveVector);
                movedPassengers.Add(passengerInfo.colliderHorizontal);
            }
        }

        // Push movables in the platform's way.
        List<CollisionController.CollisionInfo> moveablesInfos = collisionController.CheckAll(moveVector, moveableMask);
        foreach (CollisionController.CollisionInfo moveableInfo in moveablesInfos) {
            if (movedPassengers.Contains(moveableInfo.colliderHorizontal != null ? moveableInfo.colliderHorizontal : moveableInfo.colliderVertical)) {
                continue;
            }
            if (moveableInfo.colliderHorizontal != null) {
                moveableInfo.colliderHorizontal.transform.Translate(new Vector2(moveVector.x - moveableInfo.moveVector.x, 0));
            } else {
                moveableInfo.colliderVertical.transform.Translate(new Vector2(0, moveVector.y - moveableInfo.moveVector.y));
            }
        }

        transform.Translate(moveVector);
    }
}