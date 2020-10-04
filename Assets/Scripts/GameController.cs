using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    private void Awake() { 
        DontDestroyOnLoad(transform.gameObject); 
        if(GameObject.FindGameObjectsWithTag("GameController").Length > 1) Destroy(gameObject);
  }
}
