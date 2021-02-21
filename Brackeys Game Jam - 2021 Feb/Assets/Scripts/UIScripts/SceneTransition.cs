using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {
	
    public Animator transition;
    public float wait = 1f;

    public void LoadScene(int index) {
        StartCoroutine(NextScene(index));
    }

    IEnumerator NextScene(int index) {
        transition.SetTrigger("Start");

        Time.timeScale = 1;                // without this WaitForSeconds Freezes

        yield return new WaitForSeconds(wait);
        
        SceneManager.LoadScene(index);
        yield return null;
    }

}
