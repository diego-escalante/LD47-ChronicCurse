using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {
    
    public float speed = 5;
    public Vector2 minScreenPosition = new Vector2(0.4f, 0.4f);

    private Transform target;
    private float z;
    private Camera cam;

    private void Start() {
        z = transform.position.z;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(target.position.x, target.position.y + 2f, z);
        cam = transform.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update() {
        Vector3 targetPos = GetTargetPosition(target.position);
        if (transform.position == targetPos) {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        if (Vector2.Distance(transform.position, target.position) <= 0.001f) {
            transform.position = targetPos;
        }
    }

    private Vector3 GetTargetPosition(Vector3 target) {
        Vector2 viewPos = cam.WorldToViewportPoint(target);
        
        if (viewPos.x > minScreenPosition.x && viewPos.x < 1-minScreenPosition.x) {
            viewPos.x = 0.5f;
        }
        if (viewPos.y > minScreenPosition.y && viewPos.y < 1-minScreenPosition.y) {
            viewPos.y = 0.5f;
        }

        // viewPos = new Vector2(Mathf.Clamp(viewPos.x, minScreenPosition.x, 1-minScreenPosition.x), Mathf.Clamp(viewPos.y, minScreenPosition.y, 1-minScreenPosition.y));
        Vector3 worldPos = cam.ViewportToWorldPoint(viewPos);
        worldPos.z = z;
        return worldPos;

    }

    public void Translate(Vector3 delta) {
        delta.z = z;
        transform.Translate(delta);
    }
}
