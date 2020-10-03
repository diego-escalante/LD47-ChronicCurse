using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour {

    //TODO: Would be cool to make it load asynchronously. Probably post-LD task though.

    public void LoadNextScene() {
        // Loads next scene in the build order. If it's the last scene, it loads the first one.
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

}
