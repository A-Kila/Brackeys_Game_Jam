﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {

    // public GameObject inGameUI;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject helpMenu;
    public GameObject selectHandler;

    /* Help window varibles */
    public Vector2 helpPanelAlpha = new Vector2(40f, 230f);
    private Image panel;

    private GameObject activeOverlay;
    private bool isPaused = false;

    void Start() {
        activeOverlay = pauseMenu;
        panel = transform.GetChild(0).GetComponent<Image>();
    }

    public void PauseUpdate() {
        if (Input.GetKeyDown(MyInput.pause))
            if (!isPaused) OpenPause();
            else if (activeOverlay == pauseMenu) Resume();
            else Back();
    }

    public void OpenPause() {
        Time.timeScale = 0f;
        isPaused = true;
        gameObject.SetActive(true);
        selectHandler.SetActive(false);
        selectHandler.GetComponent<SelectHandler>().paused = true;
    }

    public void Resume() {
        selectHandler.SetActive(true);
        gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Options() {
        activeOverlay.SetActive(false);
        optionsMenu.SetActive(true);
        activeOverlay = optionsMenu;
    } 

    public void Help() {
        activeOverlay.SetActive(false);
        panel.color = new Color(0, 0, 0, helpPanelAlpha.y / 255);
        helpMenu.SetActive(true);
        activeOverlay = helpMenu;
    }

    public void MainMenu() {
        SceneManager.LoadSceneAsync(0);
    }

    public void Back() {
        activeOverlay.SetActive(false);
        panel.color = new Color(0, 0, 0, helpPanelAlpha.x / 255);
        pauseMenu.SetActive(true);
        activeOverlay = pauseMenu;
    }

}
