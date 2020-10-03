using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (BoxCollider2D))]
public class ExitBehavior : MonoBehaviour  {
    
    private BoxCollider2D player;
    private BoxCollider2D coll;
    private Fader fader;
    private float transitionDuration = 0.5f;
    private bool isFading = false;
    private float timeElapsed = 0;
    private SceneController sceneController;
    
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        fader = GameObject.FindGameObjectWithTag("UI").transform.Find("Fader").GetComponent<Fader>();
        sceneController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>();
        coll = GetComponent<BoxCollider2D>();

        fader.Fade(Color.black, Color.clear, transitionDuration);
    }

    private void Update() {
        if (coll.Overlaps(player) && !isFading) {
            fader.Fade(Color.clear, Color.black, transitionDuration);
            isFading = true;
        }

        if (isFading) {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= transitionDuration) {
                new SceneManager().LoadNextScene();
            }
        }
    }
}
