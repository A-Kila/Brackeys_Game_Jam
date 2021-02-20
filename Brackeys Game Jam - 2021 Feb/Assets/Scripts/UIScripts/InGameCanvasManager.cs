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

    void Start() {
        pauseMng = pause.GetComponent<PauseMenuManager>();

        upgradeText = upgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        buffSlider = buffSliderObject.GetComponentInChildren<Slider>();
        
        GameHandler.onGameWin += GameWon;
        GameHandler.onGameLose += gameLostMenu.GetComponent<GameLostManager>().GameLost;

        onBuff += DisplayBuff;
    }

    void Update() {
        pauseMng.PauseUpdate();

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
            LevelOverMenu.SetActive(true);
            gameFinishedMenu.SetActive(false);
        }
    }

}
