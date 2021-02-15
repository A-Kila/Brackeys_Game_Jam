using UnityEngine;

public class VirusManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 100;
    public Transform path;

    [HideInInspector]
    public bool isClockwizeMove = true;

    private CellMovement movement;
    private Health health;
    private Vector2[] waypoints;

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


    private int waypointIndex = 0;
    private void MoveCell() {
        Vector2 nextPos = waypoints[waypointIndex];
        movement.MoveLocation(nextPos);
        if ((Vector2)transform.position == nextPos)
            if (isClockwizeMove) waypointIndex = (waypointIndex + 1) % waypoints.Length;
            else waypointIndex = (waypointIndex - 1) % waypoints.Length;
    }

    private void PlayerDeath() {
        Destroy(gameObject);
        // Animation
        health.onPlayerDeath -= PlayerDeath;
    }
}
