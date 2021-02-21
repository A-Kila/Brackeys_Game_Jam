using UnityEngine;

public class GameHandler : MonoBehaviour {

    public static event System.Action onGameLose;
    public static event System.Action onGameWin;

    public static int virusCount = 0;
    public static int cellCount = 0;
    public static int neutralCount = 0;
    public static int money = 0;
    public static bool isGameOver = false;

    /* in Seconds */
    public float timeLimit = 300f;

    void Awake() {
        isGameOver = false;
        money = 0;
        virusCount = 0;
        cellCount = 0;
        neutralCount = 0;
    }

    void Update() {
        if (!isGameOver) { 
            if (cellCount <= 0 || timeLimit <= 0f || neutralCount <= 0) {
                if (onGameLose != null) onGameLose(); 
            } else if (virusCount <= 0)
                if (onGameWin != null) onGameWin();
        }

        timeLimit -= Time.deltaTime;
    }

}
