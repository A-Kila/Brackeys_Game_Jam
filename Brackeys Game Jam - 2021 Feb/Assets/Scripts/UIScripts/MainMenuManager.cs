using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    /* Main menu windows */
    public GameObject mainMenu;
    public GameObject levelSelector;
    public GameObject options;
    public GameObject help;

    /* Help window varibles */
    public Vector2 helpPanelAlpha = new Vector2(40f, 230f);
    public Image panel;

    private GameObject activeOverlay;

    void Start() {
        panel.color = new Color(0, 0, 0, helpPanelAlpha.x / 255); // (r,g,b) => (0,0,0) == Color.Black
        activeOverlay = mainMenu;
    }

    void Update() {
        if (Input.GetKeyDown(MyInput.pause) && activeOverlay != mainMenu)
            Back();
    }

    public void Play() {
        activeOverlay.SetActive(false);
        levelSelector.SetActive(true);
        activeOverlay = levelSelector;
    }

    public void Options() {
        activeOverlay.SetActive(false);
        options.SetActive(true);
        activeOverlay = options;
    }

    public void Help() {
        activeOverlay.SetActive(false);
        panel.color = new Color(0, 0, 0, helpPanelAlpha.y / 255);
        help.SetActive(true);
        activeOverlay = help;
    }

    public void Exit() {
        Application.Quit();
    }

    public void Back() {
        activeOverlay.SetActive(false);
        panel.color = new Color(0, 0, 0, helpPanelAlpha.x / 255);
        mainMenu.SetActive(true);
        activeOverlay = mainMenu;
    }

}
