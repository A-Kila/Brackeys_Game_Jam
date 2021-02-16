using UnityEngine;

public class VirusManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 100;
    public Transform path;

    [HideInInspector]
    public bool isClockwizeMove = true;
    [HideInInspector]
    public int cellsKilled = 0;
    [HideInInspector]
    public int waypointIndex = 0;
    [HideInInspector]
    public bool isShootingStart = false;

    private CellMovement movement;
    private Health health;
    private Vector2[] waypoints;
    private ShootProjectile projectiles;

    void OnDrawGizmos() {
        Vector2 prevPos = path.GetChild(0).position;
        
        foreach (Transform waypoint in path) {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(prevPos, waypoint.position);
            prevPos = waypoint.position;
        }

        Gizmos.DrawLine(prevPos, path.GetChild(0).position);
    }

    void Start() {
        movement = GetComponent<CellMovement>();
        movement.SetSpeed(speed);

        health = new Health(healthAmount);
        health.onPlayerDeath += PlayerDeath;

        waypoints = new Vector2[path.childCount];
        for (int i = 0; i < path.childCount; ++i) 
            waypoints[i] = path.GetChild(i).position;  
        
        projectiles = gameObject.GetComponent<ShootProjectile>();
    }
    
    private bool isVirusShooting = false;
    void Update() {
        MoveCell();
        if (!isShootingStart && (Vector2)transform.position == waypoints[0])
            isShootingStart = true;
        if (!isVirusShooting && isShootingStart) {
            isVirusShooting = true;
            StartVirusShoot();
        }
        if (cellsKilled >= 10) { 
            Duplicate(); 
            cellsKilled -= 10;
        }
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

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Friendly" || collision.collider.tag == "Neutral") {
            // Animation
            Destroy(collision.collider.gameObject);
            cellsKilled++;
        }
    }

    private void MoveCell() {
        Vector2 nextPos = waypoints[waypointIndex];
        movement.MoveLocation(nextPos);
        if ((Vector2)transform.position == nextPos)
            if (isClockwizeMove) waypointIndex = (waypointIndex + 1) % waypoints.Length;
            else waypointIndex = (waypointIndex - 1 < 0) ? waypoints.Length - 1: waypointIndex - 1;
    }

    private void PlayerDeath() {
        Destroy(gameObject);
        // Animation
        health.onPlayerDeath -= PlayerDeath;
    }

    private void StartVirusShoot() {Vector2[] shootDirections = new Vector2[4];
        float sin45 = Mathf.Sin(45f * Mathf.Deg2Rad);

        shootDirections[0] = new Vector2(sin45, sin45);  // sin(45) == cos(45)
        shootDirections[1] = new Vector2(-sin45, sin45);
        shootDirections[2] = new Vector2(-sin45, -sin45);
        shootDirections[3] = new Vector2(sin45, -sin45);

        projectiles.setDirections(shootDirections);
        projectiles.startShooting();
    }

    private void Duplicate() {
        Transform virusHolder = transform.parent.parent.GetComponent<PrefabHolder>().prefab;
        Transform cloneHolder = Instantiate(virusHolder, transform.parent.position, Quaternion.identity, transform.parent.parent);
        cloneHolder.GetChild(0).position = transform.position;

        VirusManager cloneManager = cloneHolder.GetChild(0).GetComponent<VirusManager>();

        if (isClockwizeMove) cloneManager.waypointIndex = (waypointIndex - 1) % waypoints.Length;
        else cloneManager.waypointIndex = (waypointIndex + 1) % waypoints.Length;
        cloneManager.isClockwizeMove = !isClockwizeMove;
        cloneManager.health = health;
        cloneManager.isShootingStart = true;
    }

}
