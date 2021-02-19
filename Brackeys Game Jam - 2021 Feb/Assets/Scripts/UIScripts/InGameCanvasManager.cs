using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameCanvasManager : MonoBehaviour {
	
    public static event System.Action<float> onBuff;

    public GameObject upgradeButton;
    public GameObject shopMenu;
    public GameObject pause;
    public GameObject buffSliderObject;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timer;

    private PauseMenuManager pauseMng;
    private TextMeshProUGUI upgradeText;
    private GameHandler gameHandler;
    private Slider buffSlider;

    void Start() {
        pauseMng = pause.GetComponent<PauseMenuManager>();

        upgradeText = upgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        buffSlider = buffSliderObject.GetComponentInChildren<Slider>();

        onBuff += DisplayBuff;
    }

    void Update() {
        pauseMng.PauseUpdate();

        moneyText.text = GameHandler.money.ToString() + "c";
        upgradeText.text = "UPGRADE (" + MyInput.upgrade.ToString() + ")";
        timer.text = GetTimeInMinutes();
    }

    void OnDestroy() {
        onBuff -= DisplayBuff;
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


    private string GetTimeInMinutes() {
        int minutes = (int)(gameHandler.timeLimit / 60);
        int seconds = (int)(gameHandler.timeLimit % 60);

        return (minutes.ToString() + ":" + seconds.ToString());
    }

    private void DisplayBuff(float time) {
        buffSliderObject.SetActive(true);
        StartCoroutine(DisplaySlider(time, time));
    }

    IEnumerator DisplaySlider(float startTime, float timeRemaining) {
        while (timeRemaining > 0f) {
            buffSlider.value = timeRemaining / startTime;
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        buffSliderObject.SetActive(false);
        yield return null;
    }

}
