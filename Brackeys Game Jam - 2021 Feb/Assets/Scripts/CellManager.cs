using UnityEngine;

public class CellManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 100;

    private CellMovement movement;
    private Camera gameCamera;
    private Health health;

    void Start() {
        gameCamera = FindObjectOfType<Camera>();

        movement = FindObjectOfType<CellMovement>();
        movement.SetSpeed(speed);

        health = new Health(healthAmount);
        health.onPlayerDeath += PlayerDeath;
    }

    void Update() {
        MoveCell();
    }
    void OnTriggerEnter2D(Collider2D collider) {
        // get collider.DamageAmount -- or -- collider.healthAmount
        // if (collider.tag == damage)
            health.DamageHealth(healthAmount);
        // if (collider.tag == powerup)
            // health.Addhealth(healthAmount);
        // Animation
    }


    private void MoveCell() {
        if (Input.GetMouseButtonDown(0)) { 
            Vector2 posOnWorldMap = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            movement.MoveLocation(posOnWorldMap);
        }
    }

    private void PlayerDeath() {
        Destroy(gameObject);
        // Animation
        health.onPlayerDeath -= PlayerDeath;
    }
}
