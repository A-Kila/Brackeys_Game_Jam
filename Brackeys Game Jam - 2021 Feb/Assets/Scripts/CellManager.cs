using UnityEngine;

public class CellManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 10;
 
    private bool selected = false;
    private CellMovement movement;
    private Camera gameCamera;
    private Health health;
    private ShootProjectile projectile;
    private GameObject lastVirusThatHit;

    void Start() {
        gameCamera = FindObjectOfType<Camera>();

        movement = GetComponent<CellMovement>();
        movement.SetSpeed(speed);

        health = new Health(healthAmount);
        health.onPlayerDeath += PlayerDeath;

        projectile = gameObject.GetComponent<ShootProjectile>();
    }

    void Update() {
        MoveCell();
        ShootOnMouseClick();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        ProjectileManager projectile = collider.GetComponent<ProjectileManager>();
        if (collider.tag == "Enemy") { 
            health.DamageHealth(projectile.damage);
            lastVirusThatHit = projectile.parentObj;
            // Animation
            Destroy(collider.gameObject);
        }
        // if (collider.tag == powerup) {
        //     health.Addhealth(healthAmount);
        //     Animation
        //     Destroy(collider.gameObject);
        // }
    }

    public void Deselect()
    {
        gameObject.GetComponent<ShootProjectile>().SetMarkerVisibility(false);
        selected = false;
        GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void Select(Color color)
    {
        gameObject.GetComponent<ShootProjectile>().SetMarkerVisibility(true);
        selected = true;
        GetComponent<SpriteRenderer>().color = color;
    }

    private void ShootOnMouseClick() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            projectile.setTarget(new Vector3(mousePos.x, mousePos.y, 0));
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            projectile.startShooting();
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            projectile.stopShooting();
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
        lastVirusThatHit.GetComponent<VirusManager>().cellsKilled++;
        // Animation
        health.onPlayerDeath -= PlayerDeath;
    }
}
