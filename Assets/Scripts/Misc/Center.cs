using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour {
    void Start() {
        Vector3 pos = transform.position;
        pos.x = 0;
        pos.y = 0;
        transform.position = pos;
    }
}
