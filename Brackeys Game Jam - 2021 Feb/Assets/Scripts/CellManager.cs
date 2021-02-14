using UnityEngine;

public class CellManager : MonoBehaviour {
	
    public float speed = 15f;

    private CellMovement movement;
    private Camera gameCamera;

    void Start() {
        gameCamera = FindObjectOfType<Camera>();
        movement = FindObjectOfType<CellMovement>();

        movement.SetSpeed(speed);
    }

    void Update() {
        MoveCell();
    }

    private void MoveCell() {
        if (Input.GetMouseButtonDown(0)) { 
            Vector2 posOnWorldMap = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            movement.MoveLocation(posOnWorldMap);
        }
    }
}
