using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLostManager : MonoBehaviour {

    public GameObject inGameUI;

    private SceneTransition transition;

    void Start() {
        transition = FindObjectOfType<SceneTransition>();    
    }

    public void GameLost() {
        Time.timeScale = 0f;
        GameHandler.isGameOver = true;
        gameObject.SetActive(true);
        inGameUI.SetActive(false);
    }

    public void Restart() {
        Time.timeScale = 1f;
        transition.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu() {
        Time.timeScale = 1f;
        transition.LoadScene(0);
    }

}
