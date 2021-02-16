using System.Collections;
using UnityEngine;

public class VirusManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 100;
    [Range(0f, 1f)]
    public float healthBuffDropChance = .1f;
    public Transform path;
    public Transform healthBuff;

    [HideInInspector]
    public bool isClockwizeMove = true;
    [HideInInspector]
    public int cellsKilled = 0;
    [HideInInspector]
    public int waypointIndex = 0;
    [HideInInspector]
    public bool isShootingStart = false;
    [HideInInspector]
    public Vector2[] waypoints;

    private CellMovement movement;
    private Health health;
    private ShootProjectile projectiles;
    private Vector2 screenBounds;

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

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
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
        if (collider.tag == "Friendly") { 
            ProjectileManager projectile = collider.GetComponent<ProjectileManager>();
            health.DamageHealth(projectile.damage);
            ScoreSystem.score += projectile.damage;
            //Debug.Log(ScoreSystem.score);                   // temporary

            if (Random.value <= healthBuffDropChance && healthBuffDropChance != 0)
                DropBuff(healthBuff);

            // Animation
            Destroy(collider.gameObject);
        }
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

    private void StartVirusShoot() {
        Vector2[] shootDirections = new Vector2[4];
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

    private void DropBuff(Transform buffType) {
        Transform buff = Instantiate(buffType, transform.position, Quaternion.identity);
        StartCoroutine(buffMove(buff));
    }

    IEnumerator buffMove(Transform buff) {
        float angle = Random.Range(225f, 315f);
        Vector2 buffSize = buff.GetComponent<BoxCollider2D>().size;

        float speed = 10f, acceleration = -4f;     // Forgive me father for i have sinned
        while (speed > 0) {
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
            buff.Translate(direction * speed * Time.deltaTime);
            
            if (buff.position.x - buffSize.x / 2 < -screenBounds.x ||
                buff.position.x + buffSize.x / 2 > screenBounds.x || 
                buff.position.y + buffSize.y / 2 < -screenBounds.y) 
            {
                angle = 2 * 270 - angle;
            }

            speed += acceleration * Time.deltaTime;

            yield return null;
        }
    }

}
