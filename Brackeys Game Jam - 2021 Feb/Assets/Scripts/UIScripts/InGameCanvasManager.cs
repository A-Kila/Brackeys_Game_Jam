using UnityEngine;
using TMPro;

public class InGameCanvasManager : MonoBehaviour {
	
    public GameObject upgradeButton;
    public GameObject shopMenu;
    public GameObject pause;
    public TextMeshProUGUI moneyText;

    private PauseMenuManager pauseMng;
    private TextMeshProUGUI upgradeText;

    void Start() {
        pauseMng = pause.GetComponent<PauseMenuManager>();

        upgradeText = upgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        pauseMng.PauseUpdate();

        moneyText.text = GameHandler.money.ToString() + "c";
        upgradeText.text = "UPGRADE " + MyInput.upgrade.ToString();
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
