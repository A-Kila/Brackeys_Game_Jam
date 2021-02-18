using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject levelSelector;
    public GameObject options;
    public GameObject help;

    private GameObject activeOverlay;

    void Start() {
        activeOverlay = mainMenu;
    }

    public void Play() {
        activeOverlay.SetActive(false);
        levelSelector.SetActive(true);
    }

    public void Options() {
        activeOverlay.SetActive(false);
        options.SetActive(true);
    }

    public void Help() {
        activeOverlay.SetActive(false);
        help.SetActive(true);
    }

    public void Exit() {
        Application.Quit();
    }

}
