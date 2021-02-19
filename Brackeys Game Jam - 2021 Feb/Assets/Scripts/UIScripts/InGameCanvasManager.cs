using System.Collections;
using UnityEngine;

public class InGameCanvasManager : MonoBehaviour {
	
    public GameObject pause;

    private PauseMenuManager pauseMng;

    void Start() {
        pauseMng = pause.GetComponent<PauseMenuManager>();
    }

    void Update() {
        pauseMng.PauseUpdate();
    }

}
