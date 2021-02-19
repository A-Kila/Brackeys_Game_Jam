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
    [HideInInspector]
    public bool shooting = false;

    private CellMovement movement;
    private GameObject lastVirusThatHit;
    private SpriteRenderer HighlightSprite;
    

    void Start() {
        GameHandler.cellCount++;

        HighlightSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        HighlightSprite.color = new Color(0,0,0, 0);

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

    private void FixedUpdate()
    {
        if(selected && !shooting)
        {
            Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dif = mousPos - (Vector2)transform.position;
            float angel = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angel);
        }
        
    }

    public void changeDirection(Vector2 targetObj)
    {
        Vector2 dif = targetObj - (Vector2)transform.position;
        float degree = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, degree);
    }

    public void Deselect()
    {
        if(deSelectFuncs != null) deSelectFuncs();
        selected = false;
        HighlightSprite.color = new Color(0, 0, 0, 0);
    }
    public void Select(Color color)
    {
        if(selectFuncs != null)selectFuncs();
        selected = true;
        HighlightSprite.color = color;
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
