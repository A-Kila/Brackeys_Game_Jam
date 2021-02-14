using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private Vector3 screenBounds;
    public void setup(Vector3 dir)
    {
        direction = dir;
    }
    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        transform.position += direction * Time.deltaTime * speed;
        if (transform.position.x > screenBounds.x || transform.position.y > screenBounds.y ||
            transform.position.x < - screenBounds.x || transform.position.y < - screenBounds.y) //Checks if projectile is visible by Camera
            Destroy(gameObject);
    }   
}
