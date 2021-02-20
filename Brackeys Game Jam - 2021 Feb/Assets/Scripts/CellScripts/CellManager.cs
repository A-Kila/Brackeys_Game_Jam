using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CellManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 10;
    public float highlightIntensity = 1.5f;

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
    private Light2D Highlight;

    public HealthBarControler healthBar;

    void Start() {
        GameHandler.cellCount++;

        Highlight = transform.GetChild(0).GetComponent<Light2D>();
        Highlight.intensity = 0;
        HighlightSprite = Highlight.transform.GetChild(0).GetComponent<SpriteRenderer>();
        HighlightSprite.color = new Color(0, 0, 0, 0);

        movement = GetComponent<CellMovement>();
        movement.SetSpeed(speed);

        health = new Health(healthAmount, healthBar);
        health.onPlayerDeath += PlayerDeath;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            ProjectileManager projectile = collider.GetComponent<ProjectileManager>();
            lastVirusThatHit = projectile.parentObj;
            health.DamageHealth(projectile.damage);
            // Animation
            FindObjectOfType<AudioManager>().Play("BulletHit");
            Destroy(collider.gameObject);
        }
        if (collider.tag == "Buff") {
            foreach (Transform child in transform.parent)
                collider.GetComponent<BuffManager>().onCollideDoAction(child);
        //    Animation
            FindObjectOfType<AudioManager>().Play("Buff");
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

    private void rotate(float degree)
    {
        Transform[] children = new Transform[this.transform.childCount];
        int i = 0;
        foreach (Transform child in this.transform)
        {
            children[i++] = child;
        }
        // Detach
        this.transform.DetachChildren();

        // Change parent transform
        this.transform.eulerAngles =  new Vector3(0, 0, degree);

        // Reparent
        foreach (Transform child in children)
        {
            child.parent = this.transform;
        }

        children = null;
    }

    public void Deselect()
    {
        if(deSelectFuncs != null) deSelectFuncs();
        selected = false;
        movement.setTargetVisibility(false);
        Highlight.intensity = 0;
        HighlightSprite.color = new Color(0, 0, 0, 0);
    }
    public void Select(Color color)
    {
        if(selectFuncs != null)selectFuncs();
        selected = true;
        Debug.Log(color);
        Highlight.intensity = highlightIntensity;
        movement.setTargetVisibility(true);
        //Highlight.color = color;
        HighlightSprite.color = color;
    }
    public void PlayerDeath() {
        if(lastVirusThatHit != null) lastVirusThatHit.GetComponent<VirusManager>().cellsKilled++;

        ExplosionHandler noUse;;
        if (!transform.TryGetComponent(out noUse))
            FindObjectOfType<AudioManager>().Play("CellDeath");

        // Animation
        health.onPlayerDeath -= PlayerDeath;

        if (transform.parent.childCount == 1) Destroy(transform.parent.gameObject);
        Destroy(gameObject);
        GameHandler.cellCount--;
        if (onPlayerDeathFuncs != null) onPlayerDeathFuncs();
    }
}
