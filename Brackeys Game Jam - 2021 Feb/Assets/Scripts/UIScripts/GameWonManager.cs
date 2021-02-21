using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWonManager : MonoBehaviour {

    private SceneTransition transition;

    void Start() {
        transition = FindObjectOfType<SceneTransition>();    
    }

    public void NextLevel() {
        Time.timeScale = 1f;
        transition.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
