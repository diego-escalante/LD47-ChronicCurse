using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLooper : MonoBehaviour, ILooper {
    int waypointIndex;
    private PlatformController controller;
    private SpriteRenderer loopGhost;
    private SpriteRenderer rend;
    public Sprite loopSprite;

    public void OnEnable() {
        this.SubscribeToLoop();
        rend.sprite = loopSprite;
    }

    public void OnDisable() {
        this.UnsubscribeFromLoop();
    }

    public void Awake() {
        rend = GetComponent<SpriteRenderer>();
    }

    public void SetState() {
        waypointIndex = GetController().GetCurrentWaypointIndex();
        createLoopGhost();
    }

    public void Loop() {
        GetController().SetCurrentWaypoint(waypointIndex);
    }

    public bool IsLooping() {
        return true;
    }

    private PlatformController GetController() {
        if (controller == null) {
            controller = GetComponent<PlatformController>();
        }
        return controller;
    }

    private void createLoopGhost() {
        // Create ghost if it doesn't exist.
        if (loopGhost == null) {
            GameObject ghostGO = new GameObject(gameObject.name + " Loop Ghost");
            loopGhost = ghostGO.AddComponent<SpriteRenderer>();
            Color c = loopGhost.color;
            c.a = 0.5f;
            loopGhost.color = c;
        }
        loopGhost.sprite = rend.sprite;
        loopGhost.drawMode = rend.drawMode;
        loopGhost.size = rend.size;
        loopGhost.transform.position = transform.position;
    }
}
