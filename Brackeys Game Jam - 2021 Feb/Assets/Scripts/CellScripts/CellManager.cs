using UnityEngine;

public class CellManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 10;

    [HideInInspector]
    public Health health;
 
    private bool selected = false;
    private CellMovement movement;
    private Camera gameCamera;
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
        ShootOnMouseClick();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            ProjectileManager projectile = collider.GetComponent<ProjectileManager>();
            lastVirusThatHit = projectile.parentObj;
            health.DamageHealth(projectile.damage);
            // Animation
            Destroy(collider.gameObject);
        }
        if (collider.tag == "Buff") {
            foreach (Transform child in transform.parent)
                collider.GetComponent<BuffManager>().onCollideDoAction(child);
        //    Animation
            Destroy(collider.gameObject);
        }
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
        if (!selected) return;
        if (Input.GetKeyDown(MyInput.targetSelect)) {

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] targetCollider = Physics2D.OverlapAreaAll(mousePos, mousePos);

            if (targetCollider.Length == 0)
                projectile.setTarget(new Vector3(mousePos.x, mousePos.y, 0));
            else
                projectile.setTarget(targetCollider[0].gameObject);
        
        }
        if (Input.GetKeyDown(MyInput.startShoot)) {
            projectile.startShooting();
        }
        if (Input.GetKeyDown(MyInput.stopShoot)) {
            projectile.stopShooting();
        }
    }

    private void PlayerDeath() {
        lastVirusThatHit.GetComponent<VirusManager>().cellsKilled++;
        // Animation
        health.onPlayerDeath -= PlayerDeath;

        if (transform.parent.childCount == 1) Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }
}
