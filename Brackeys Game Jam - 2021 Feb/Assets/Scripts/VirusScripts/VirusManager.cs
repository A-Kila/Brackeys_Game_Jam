using System.Collections;
using UnityEngine;

public class VirusManager : MonoBehaviour {
	
    public float speed = 15f;
    public float collisionNeeded = 5f;
    public int collisionDamage = 100;
    public int healthAmount = 100;
    public bool rotate = false;
    public float rotateDelay = 1f;
    [Range(0f, 360f)]
    public float rotateAngel = 0f;
    [Range(0f, 1f)]
    public float healthBuffDropChance = .1f;
    public Transform path;
    public Transform healthBuff;
    public int shootDirectionNum = 4;
    public float shootDirectionRotation = 45f;
    public Vector2 shootArc = new Vector2(0, 360);

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
    [HideInInspector]
    public int colliderCount = 0;

    private CellMovement movement;
    private Rigidbody2D rb;
    [HideInInspector]
    public Health health;
    private ShootProjectile projectiles;
    private float lastRotateTime;

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

        rb = GetComponent<Rigidbody2D>();
        lastRotateTime = Time.time;
    }
    
    private bool isVirusShooting = false;
    void FixedUpdate() {
        Debug.Log(colliderCount);
        if (colliderCount != 0) slowDown(colliderCount);
        else movement.SetSpeed(speed);
            MoveCell();
            Rotate();
        if (!isShootingStart && waypointIndex > 0)
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

    private void Rotate()
    {
        if (!rotate) return;
        float currTime = Time.time;
        if(currTime - lastRotateTime> rotateDelay)
        {
            lastRotateTime = currTime;
            transform.eulerAngles += new Vector3(0, 0, rotateAngel);
            StartVirusShoot();
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Friendly") { 
            ProjectileManager projectile = collider.GetComponent<ProjectileManager>();
            health.DamageHealth(projectile.damage);
            ScoreSystem.score += projectile.damage;
            // Log Score

            if (Random.value <= healthBuffDropChance && healthBuffDropChance != 0)
                DropBuff(healthBuff);

            // Animation
            Destroy(collider.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Friendly" || collision.collider.tag == "Neutral") {
            // Animation
            collision.collider.gameObject.GetComponent<CellManager>().health.DamageHealth(collisionDamage);
        }
    }

    private void slowDown(int i)
    {
        float newSpeed = speed / ((float)i/ collisionNeeded);
        if (newSpeed > speed) newSpeed = speed;
        // Debug.Log(newSpeed);
        movement.SetSpeed(newSpeed);
    }

    private void MoveCell() {
        Vector2 nextPos = waypoints[waypointIndex];
        movement.MoveLocation(nextPos);
        if ((Vector2)transform.position == nextPos)    
        {
           
            if (isClockwizeMove) waypointIndex = (waypointIndex + 1) % waypoints.Length;
            else waypointIndex = (waypointIndex - 1 < 0) ? waypoints.Length - 1 : waypointIndex - 1;
            if(!rotate) changeDirection();
        }
    }

    private void PlayerDeath() {
        Destroy(gameObject);
        // Animation
        health.onPlayerDeath -= PlayerDeath;
    }

    private void changeDirection()
    {
        Vector2 dif = waypoints[waypointIndex] - rb.position;
        float degree = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, degree); 
        StartVirusShoot();
    }
    private void StartVirusShoot() {
        Vector2[] shootDirections = new Vector2[shootDirectionNum];

        float arcLength = shootArc.y - shootArc.x;

        for(int i = 0; i < shootDirectionNum; ++i)
        {
            float rotation = transform.eulerAngles.z;
            if (rotation < 0) rotation += 360;
            float degree = ( rotation + shootArc.x + shootDirectionRotation + i * arcLength / shootDirectionNum );
            Debug.Log(degree);
            float sin = Mathf.Sin(degree * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degree * Mathf.Deg2Rad);
            Debug.Log(sin + " " + cos);
            shootDirections[i] = new Vector2(cos, sin);
        }

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
    }

}
