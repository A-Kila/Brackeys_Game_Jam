using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMovement : MonoBehaviour {
	
    public float speed = 15f;

    private Camera gameCamera;
    private Rigidbody2D rb;

    private Vector2 moveTowards;


    void Start() {
        gameCamera = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody2D>();
        moveTowards = transform.position;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
            moveTowards = gameCamera.ScreenToWorldPoint((Input.mousePosition));         // Get the directions from a cell to mouse position
    }

    void FixedUpdate() {
        rb.MovePosition(Vector3.MoveTowards(rb.position, moveTowards, speed * Time.deltaTime));
    }

}
