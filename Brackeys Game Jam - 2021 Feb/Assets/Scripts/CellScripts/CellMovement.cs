using UnityEngine;

public class CellMovement : MonoBehaviour {

    [HideInInspector]
    public float speed;
    private Rigidbody2D rb;

    [HideInInspector]
    public Vector2 moveTowards;
    private float halfWidth, halfHeight, halfRBRadius;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        moveTowards = transform.position;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
        halfRBRadius = rb.transform.localScale.x / 2;   
    }

    void FixedUpdate() {
        rb.MovePosition(Vector3.MoveTowards(rb.position, moveTowards, speed * Time.deltaTime));
    }

    public void MoveLocation(Vector2 moveLocation) {
        moveLocation.x = Mathf.Clamp(moveLocation.x, -halfWidth + halfRBRadius, halfWidth - halfRBRadius);
        moveLocation.y = Mathf.Clamp(moveLocation.y, -halfHeight + halfRBRadius, halfHeight - halfRBRadius);
        moveTowards = moveLocation;
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

}
