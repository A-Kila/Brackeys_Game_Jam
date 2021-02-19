using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameWonManager : MonoBehaviour {
	
    public GameObject inGameUI;
    public GameObject LevelOverMenu;
    public GameObject gameFinishedMenu;

    void Start() {
        GameHandler.onGameWin += GameWon;
    }

    void OnDestroy() {
        GameHandler.onGameWin -= GameWon;
    }

    private void GameWon() {
        Image panel = transform.GetChild(0).GetComponent<Image>();
        Time.timeScale = 0f;

        inGameUI.SetActive(false);
        gameObject.SetActive(true);
        
        if (SceneManager.GetActiveScene().buildIndex == Levels.numLevels) {
            LevelOverMenu.SetActive(false);
            gameFinishedMenu.SetActive(true);
            panel.color = new Color(0f, 0f, 0f, 1f); // Black
        } else { 
            LevelOverMenu.SetActive(true);
            gameFinishedMenu.SetActive(false);
        }
    }

    public void NextLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
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
