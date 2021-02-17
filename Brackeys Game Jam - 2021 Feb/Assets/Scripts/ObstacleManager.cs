using UnityEngine;

public class ObstacleManager : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider) {
        Destroy(collider.gameObject);
    }

}
