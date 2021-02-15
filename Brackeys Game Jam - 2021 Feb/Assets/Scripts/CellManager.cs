using UnityEngine;

public class CellManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 100;
    
 
    private bool selected;
    private CellMovement movement;
    private Camera gameCamera;
    private Health health;

    void Start() {
        gameCamera = FindObjectOfType<Camera>();

        movement = GetComponent<CellMovement>();
        movement.SetSpeed(speed);

        selected = false;

        health = new Health(healthAmount);
        health.onPlayerDeath += PlayerDeath;
    }

    void Update() {
        MoveCell();
        ShootOnMouseClick();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // get collider.DamageAmount -- or -- collider.healthAmount
        if (collider.tag == "Enemy")
            health.DamageHealth(healthAmount);
        // if (collider.tag == powerup)
            // health.Addhealth(healthAmount);
        // Animation
    }

    public void Deselect()
    {
        gameObject.GetComponent<ShootProjectile>().SetMarkerVisibility(false);
        selected = false;
    }
    public void Select()
    {
        gameObject.GetComponent<ShootProjectile>().SetMarkerVisibility(true);
        selected = true;
    }

    private void ShootOnMouseClick() {
        if (!selected) return;
        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.GetComponent<ShootProjectile>().setTarget(new Vector3(mousePos.x, mousePos.y, 0));
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            gameObject.GetComponent<ShootProjectile>().startShooting();
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            gameObject.GetComponent<ShootProjectile>().stopShooting();
        }
    }

    private void MoveCell() {
        if (!selected) return;
        if (Input.GetMouseButtonDown(1)) {
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
