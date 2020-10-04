using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour {

    //TODO: Would be cool to make it load asynchronously. Probably post-LD task though.
    private DialogueManager dialogueManager;

    private void Start() {
        dialogueManager = GetComponent<DialogueManager>();
    }

    private void Update() {
        if (!dialogueManager.IsOpen()) {
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                LoadPreviousScene();
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                LoadNextScene();
            } else if (Input.GetKeyDown(KeyCode.R)) {
                RestartScene();
            }
        }
    }

    public void LoadNextScene() {
        // Loads next scene in the build order. If it's the last scene, it loads the first one.
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    public void LoadPreviousScene() {
        int index = (SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(index < 0 ? SceneManager.sceneCountInBuildSettings - 1 : index);
    }

    public void RestartScene() {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
    }

}
