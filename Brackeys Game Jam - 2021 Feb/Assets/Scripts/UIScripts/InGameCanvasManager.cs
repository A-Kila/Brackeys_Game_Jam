using UnityEngine;
using TMPro;

public class InGameCanvasManager : MonoBehaviour {
	
    public GameObject upgradeButton;
    public GameObject shopMenu;
    public GameObject pause;
    public TextMeshProUGUI text;

    private PauseMenuManager pauseMng;

    void Start() {
        pauseMng = pause.GetComponent<PauseMenuManager>();
    }

    void Update() {
        pauseMng.PauseUpdate();

        text.text = GameHandler.money.ToString() + "c";
    }

    public void Upgrade() {
        Time.timeScale = 0f;
        upgradeButton.SetActive(false);
        shopMenu.SetActive(true);
    }

    public void Back() {
        Time.timeScale = 1f;
        upgradeButton.SetActive(true);
        shopMenu.SetActive(false);
    }

}
