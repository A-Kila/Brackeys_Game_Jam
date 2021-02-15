using UnityEngine;

public class VirusManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 100;
    public Transform pathHolder;

    private CellMovement movement;
    private Health health;

    void Start() {
        movement = GetComponent<CellMovement>();
        movement.SetSpeed(speed);

        health = new Health(healthAmount);
        health.onPlayerDeath += PlayerDeath;
    }

    void Update() {
        MoveCell();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        int projectileDamage = collider.GetComponent<ProjectileManager>().damage;
        if (collider.tag == "Friendly") { 
            health.DamageHealth(projectileDamage);
            // Animation
            Destroy(collider.gameObject);
        }
        // if (collider.tag == powerup) {
        //     health.Addhealth(healthAmount);
        //     Animation
        //     Destroy(collider.gameObject);
        // }
    }

    private void MoveCell() { /* to do */ }

    private void PlayerDeath() {
        Destroy(gameObject);
        // Animation
        health.onPlayerDeath -= PlayerDeath;
    }
}
