using UnityEngine;

public class BuffManager : MonoBehaviour {

    private float angle;
    private float speed = 16f, acceleration = -16f;          // Forgive me father for i have sinned
    private Vector2 buffSize;
    private  Vector2 screenBounds;

    void Start() {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        angle = Random.Range(225f, 315f);
        buffSize = transform.GetComponent<BoxCollider2D>().size;
    }

    void Update() {
        if (speed > 0)
            BuffAnimation();
    }

    public System.Action<Transform> onCollide;

    public void onCollideDoAction(Transform objTransform) {
        if (onCollide != null)
            onCollide(objTransform);
    } 

    private void BuffAnimation() {
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
            
        if (transform.position.x - buffSize.x / 2 < -screenBounds.x ||
            transform.position.x + buffSize.x / 2 > screenBounds.x || 
            transform.position.y + buffSize.y / 2 < -screenBounds.y) 
        {
            angle = 2 * 270 - angle;
        }

        speed += acceleration * Time.deltaTime;
    }

}
