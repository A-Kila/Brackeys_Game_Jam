using UnityEngine;

public class CellMovement : MonoBehaviour {
	
    public float speed = 15f;
    private Rigidbody2D rb;

    private Vector2 moveTowards;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        moveTowards = transform.position;
    }

    void FixedUpdate() {
        rb.MovePosition(Vector3.MoveTowards(rb.position, moveTowards, speed * Time.deltaTime));
    }

    public void MoveLocation(Vector2 moveLocation) {
        moveTowards = moveLocation;
    }

}
