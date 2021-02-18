using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    private GameObject[] keybindButtons;

    void Start() {
        panel.color = new Color(0, 0, 0, helpPanelAlpha.x / 255); // (r,g,b) => (0,0,0) == Color.Black
        activeOverlay = mainMenu;

        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        foreach (GameObject keybind in keybindButtons) {
            TextMeshProUGUI text = keybind.GetComponentInChildren<TextMeshProUGUI>();
            GameObject button = keybind.transform.GetChild(1).gameObject;
            button.GetComponentInChildren<TextMeshProUGUI>().text = GetKeybind(text.text).ToString();
        }

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

    /* Options menu buttons */
    public void MusicSlider() {}

    public void SFXSlider() {}

    public void Select() { AssignNewKey("Select"); }
    public void Move() { AssignNewKey("Move"); }
    public void StartShooting() { AssignNewKey("Start Shooting"); }
    public void StopShooting() { AssignNewKey("Stop Shooting"); }
    public void CombineGroups() { AssignNewKey("Combine Groups"); }
    public void DivideGroups() { AssignNewKey("Divide Groups"); }
    public void TargetSelection() { AssignNewKey("Target Selection"); }
    public void OpenShop() { AssignNewKey("Open Shop"); }

    /* Options pricate methods */
    private KeyCode GetKeybind(string action) {
        KeyCode result = KeyCode.None;
        switch (action) {
            case "Select": 
                result =  MyInput.select;
                break;
            case "Move": 
                result =  MyInput.move;
                break;
            case "Start Shooting": 
                result =  MyInput.startShoot;
                break;
            case "Stop Shooting": 
                result =  MyInput.stopShoot;
                break;
            case "Combine Groups": 
                result =  MyInput.combineGroups;
                break;
            case "Divide Groups": 
                result =  MyInput.divideGroups;
                break;
            case "Target Selection": 
                result =  MyInput.targetSelect;
                break;
            case "Open Shop": 
                result =  MyInput.openShop;
                break;
        }
        return result;
    }

    private void AssignNewKey(string name) {
        KeyCode key = GetKeybind(name);
        while (true) {
            Event e = Event.current;
            if (e != null && e.isKey) {
                key = e.keyCode;

                foreach (GameObject keybind in keybindButtons) {
                    TextMeshProUGUI text = keybind.GetComponentInChildren<TextMeshProUGUI>();
                    if (text.text == name) { 
                        GameObject button = keybind.transform.GetChild(1).gameObject;
                        button.GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
                    }
                }
            }
        }
    }

}
