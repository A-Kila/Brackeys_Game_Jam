using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class NeutralManager : MonoBehaviour
{

    public float speed;
    public float minSpeed= 10f, maxSpeed= 20f;
    public float collisionNeeded = 5f;
    public int healthAmount = 100;
    public ParticleSystem DeathParticle;

    [HideInInspector]
    public int colliderCount = 0;

    private CellMovement movement;
    [HideInInspector]
    public Health health;
    private Rigidbody2D rb;
    private int index;
    private NeutralGroupManager ngm;
    private int waypointIndex = 0;
    [HideInInspector]
    public bool isClockwizeMove = true;
    public HealthBarControler HeatlthBar;



    void Start()
    {
        GameHandler.neutralCount++;
        speed = Random.Range(minSpeed, maxSpeed);

        movement = GetComponent<CellMovement>();
        movement.SetSpeed(speed);

        health = new Health(healthAmount, HeatlthBar);
        health.onPlayerDeath += cellDeath;

        index = transform.GetSiblingIndex();
        rb = GetComponent<Rigidbody2D>();
        ngm = GetComponentInParent<NeutralGroupManager>();
    }

    void FixedUpdate()
    {
        if (colliderCount != 0) slowDown(colliderCount);
        else movement.SetSpeed(speed);
        MoveCell();
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(health.health);
        ProjectileManager projectile = collider.GetComponent<ProjectileManager>();
        if (projectile == null) return;
         health.DamageHealth(projectile.damage);
          projectile.DestroyProjectile();
    }


    private void slowDown(int i)
    {
        float newSpeed = speed / ((float)i / collisionNeeded);
        if (newSpeed > speed) newSpeed = speed;
        // Debug.Log(newSpeed);
        movement.SetSpeed(newSpeed);
    }

    public void MoveCell()
    {
        Vector2 nextPos = ngm.getNextPoint(waypointIndex, index);
        movement.MoveLocation(nextPos);
        if (Mathf.Abs(rb.position.x - nextPos.x) < 0.01f && Mathf.Abs(rb.position.y - nextPos.y) < 0.01f)
        {

            if (isClockwizeMove) waypointIndex = (waypointIndex + 1) % ngm.waypoints.Length;
            else waypointIndex = (waypointIndex - 1 < 0) ? ngm.waypoints.Length - 1 : waypointIndex - 1;
             changeDirection();
        }
    }

    private void cellDeath()
    {
        // ParticleSystem tp = Instantiate(DeathParticle, transform.position, Quaternion.identity);
        // tp.Play();
       // Destroy(tp.gameObject, tp.main.duration);
        Destroy(gameObject);
        GameHandler.neutralCount--;
        // Animation
        health.onPlayerDeath -= cellDeath;
    }

    private void changeDirection()
    {
        Vector2 dif = ngm.getNextPoint(waypointIndex, index) - (Vector2)transform.position;
        float degree = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, degree);
    }

}
