using UnityEngine;

public class CellManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 10;

    [HideInInspector]
    public Health health;
    [HideInInspector]
    public bool selected = false;
    [HideInInspector]
    public System.Action selectFuncs;
    [HideInInspector]
    public System.Action deSelectFuncs;
    [HideInInspector]
    public System.Action stopActionsFuncs;
    [HideInInspector]
    public System.Action onPlayerDeathFuncs;

    private CellMovement movement;
    private GameObject lastVirusThatHit;

    void Start() {
        GameHandler.cellCount++;

        movement = GetComponent<CellMovement>();
        movement.SetSpeed(speed);

        health = new Health(healthAmount);
        health.onPlayerDeath += PlayerDeath;
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
        if(deSelectFuncs != null) deSelectFuncs();
        selected = false;
        GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void Select(Color color)
    {
        if(selectFuncs != null)selectFuncs();
        selected = true;
        GetComponent<SpriteRenderer>().color = color;
    }


    public void PlayerDeath() {
        if(lastVirusThatHit != null) lastVirusThatHit.GetComponent<VirusManager>().cellsKilled++;
        // Animation
        health.onPlayerDeath -= PlayerDeath;

        if (transform.parent.childCount == 1) Destroy(transform.parent.gameObject);
        Destroy(gameObject);
        GameHandler.cellCount--;
        if (onPlayerDeathFuncs != null) onPlayerDeathFuncs();
    }
}
