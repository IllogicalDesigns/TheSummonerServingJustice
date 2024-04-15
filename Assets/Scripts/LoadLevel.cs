using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {
    public void LoadALevel(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void RestartCurrentLevel() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
