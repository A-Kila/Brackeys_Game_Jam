﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameCanvasManager : MonoBehaviour {
	
    public static event System.Action<float> onBuff;
	
    public GameObject upgradeButton;
    public GameObject shopMenu;
    public GameObject inGameUI;
    public GameObject pause;
    public GameObject gameWonMenu;
    public GameObject LevelOverMenu;
    public GameObject gameFinishedMenu;
    public GameObject gameLostMenu;
    public GameObject buffSliderObject;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timer;
    
    private PauseMenuManager pauseMng;
    private TextMeshProUGUI upgradeText;
    private GameHandler gameHandler;
    private Slider buffSlider;
    private GameObject selectHandler;

    void Start() {
        pauseMng = pause.GetComponent<PauseMenuManager>();

        upgradeText = upgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        buffSlider = buffSliderObject.GetComponentInChildren<Slider>();
        selectHandler = GameObject.FindGameObjectWithTag("SelectHandler");
        
        GameHandler.onGameWin += GameWon;
        GameHandler.onGameLose += gameLostMenu.GetComponent<GameLostManager>().GameLost;

        onBuff += DisplayBuff;
    }

    public static void InvokeOnBuff(float time)
    {
        if (onBuff != null)
            onBuff(time);
    }

    void Update() {
        pauseMng.PauseUpdate();

        UpgradeUpdate();

        moneyText.text = GameHandler.money.ToString();
        upgradeText.text = "UPGRADE (" + MyInput.upgrade.ToString() + ")";
        timer.text = GetTimeInMinutes();
    }

    void OnDestroy() {
        onBuff -= DisplayBuff;
        
        GameHandler.onGameWin -= GameWon;
        GameHandler.onGameLose -= gameLostMenu.GetComponent<GameLostManager>().GameLost;
    }

    public void Upgrade() {
        if (GameHandler.isGameOver) return;

        Time.timeScale = 0f;
        upgradeButton.SetActive(false);
        shopMenu.SetActive(true);
        // selectHandler.SetActive(false);
        selectHandler.GetComponent<SelectHandler>().paused = true;
    }

    public void Back() {
        // selectHandler.SetActive(true);
        Time.timeScale = 1f;
        upgradeButton.SetActive(true);
        shopMenu.SetActive(false);
    }
    
    private string GetTimeInMinutes() {
        int minutes = (int)(gameHandler.timeLimit / 60);
        int seconds = (int)(gameHandler.timeLimit % 60);

        return (minutes.ToString() + ":" + seconds.ToString());
    }

    private IEnumerator coroutine;
    private void DisplayBuff(float time) {
        if (buffSliderObject.activeSelf) StopCoroutine(coroutine);
        buffSliderObject.SetActive(true);
        coroutine = DisplaySlider(time, time);
        StartCoroutine(coroutine);
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

    private void GameWon() {
        Time.timeScale = 0f;
        GameHandler.isGameOver = true;

        inGameUI.SetActive(false);
        gameWonMenu.SetActive(true);

        Image panel = gameWonMenu.transform.GetChild(0).GetComponent<Image>();
        
        if (SceneManager.GetActiveScene().buildIndex == Levels.numLevels) {
            LevelOverMenu.SetActive(false);
            gameFinishedMenu.SetActive(true);
            panel.color = new Color(0f, 0f, 0f, 1f); // Black
        } else { 
            Levels.isUnlocked[SceneManager.GetActiveScene().buildIndex] = true;  // builIndex = arrayIndex + 1, this code unlockes the next level
            LevelOverMenu.SetActive(true);
            gameFinishedMenu.SetActive(false);
        }
    }

    private void UpgradeUpdate() {
        if (Input.GetKeyDown(MyInput.upgrade)) {
            if (!shopMenu.activeSelf) Upgrade();
            else Back();
        }
    }

}
