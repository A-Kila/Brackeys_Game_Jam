using UnityEngine;
using TMPro;

public class InGameCanvasManager : MonoBehaviour {
	
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

}
