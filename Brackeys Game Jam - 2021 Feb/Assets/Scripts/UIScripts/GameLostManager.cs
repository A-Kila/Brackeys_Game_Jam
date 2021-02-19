using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLostManager : MonoBehaviour {

    void Start() {
        GameHandler.onGameLose += GameLost;
    }

    void OnDestroy() {
        GameHandler.onGameLose -= GameLost;
    }

    private void GameLost() {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

}
