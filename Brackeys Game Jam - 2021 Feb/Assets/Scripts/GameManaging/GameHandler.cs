using UnityEngine;

public class GameHandler : MonoBehaviour {

    public static event System.Action onGameLose;
    public static event System.Action onGameWin;

    public static int virusCount = 0;
    public static int cellCount = 0;
    public static int money = 0;
    public static bool isGameOver = false;

    /* in Seconds */
    public float timeLimit = 300f;

    void Awake() {
        Time.timeScale = 1f;
        isGameOver = false;
        money = 0;
    }

    void Update() {
        if (cellCount <= 0 || timeLimit <= 0f) {
            if (onGameLose != null) onGameLose();
        } else if (virusCount <= 0)
                onGameWin();

        timeLimit -= Time.deltaTime;
    }

}
