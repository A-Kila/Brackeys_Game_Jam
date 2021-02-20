using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLostManager : MonoBehaviour {

    public GameObject inGameUI;

    public void GameLost() {
        Time.timeScale = 0f;
        GameHandler.isGameOver = true;
        gameObject.SetActive(true);
        inGameUI.SetActive(false);
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
