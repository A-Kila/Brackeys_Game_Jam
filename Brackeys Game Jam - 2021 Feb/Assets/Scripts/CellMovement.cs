using UnityEngine;

public class CellMovement : MonoBehaviour {
	
    private float speed;
    private Rigidbody2D rb;

    private Vector2 moveTowards;
    private Vector2 nextPosition;
    private float halfWidth, halfHeight, halfRBRadius;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        moveTowards = transform.position;
        nextPosition = moveTowards;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
        halfRBRadius = rb.transform.localScale.x / 2;   
    }

    void FixedUpdate() {
        nextPosition = Vector2.MoveTowards(rb.position, moveTowards, speed * Time.deltaTime);
        if (rb.isKinematic) transform.position = nextPosition;
        else rb.MovePosition(nextPosition);
    }

    public void MoveLocation(Vector2 moveLocation) {
        if (!(rb.isKinematic)) { 
            moveLocation.x = Mathf.Clamp(moveLocation.x, -halfWidth + halfRBRadius, halfWidth - halfRBRadius);
            moveLocation.y = Mathf.Clamp(moveLocation.y, -halfHeight + halfRBRadius, halfHeight - halfRBRadius);
        }
        moveTowards = moveLocation;
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

}
