using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWonManager : MonoBehaviour {

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
