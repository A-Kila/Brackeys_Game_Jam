using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorManager : MonoBehaviour {
	
    private GameObject[] levelButtons;
    private Vector3 unlockedColor = new Vector3(255f, 255f, 255f); // white
    private Vector3 lockedColor = new Vector3(150f, 150f, 150f); // grey


    void Start() {
        levelButtons = GameObject.FindGameObjectsWithTag("LevelSelector");

        for (int i = 0; i < levelButtons.Length; i++) {

            Vector3 c;
            if (Levels.isUnlocked[i]) c = unlockedColor;
            else c = lockedColor;
        
            c /= 255f;
            levelButtons[i].GetComponent<Image>().color = new Color(c.x, c.y, c.z);
        }
    }

    public void Level1() { GoToLevel(1); }
    public void Level2() { GoToLevel(2); }
    public void Level3() { GoToLevel(3); }
    public void Level4() { GoToLevel(4); }
    public void Level5() { GoToLevel(5); }
    public void Level6() { GoToLevel(6); }
    public void Level7() { GoToLevel(7); }
    public void Level8() { GoToLevel(8); }
    public void Level9() { GoToLevel(9); }
    public void Level10() { GoToLevel(10); }
    public void Level11() { GoToLevel(11); }
    public void Level12() { GoToLevel(12); }

    private void GoToLevel(int level) {
        if (Levels.isUnlocked[level - 1])
            SceneManager.LoadSceneAsync(level);
    }

}
