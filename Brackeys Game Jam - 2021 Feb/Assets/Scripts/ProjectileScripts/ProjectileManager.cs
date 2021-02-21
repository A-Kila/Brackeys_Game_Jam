using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public ParticleSystem ProjectileParticle;

    [HideInInspector]
    public System.Action beforeDestroy;

    [HideInInspector]
    public GameObject parentObj;

    private Vector3 direction;
    private Vector3 screenBounds;
    public void setup(Vector3 dir)
    {
        direction = dir;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TrailRenderer>().sortingLayerName = "Projectile";
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    public void DestroyProjectile()
    {
        ParticleSystem tmp = Instantiate(ProjectileParticle, transform.position, Quaternion.identity);
        tmp.Play();
        if (beforeDestroy != null) beforeDestroy();
        Destroy(tmp.gameObject, tmp.main.duration);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
        if (transform.position.x > screenBounds.x || transform.position.y > screenBounds.y ||
            transform.position.x < - screenBounds.x || transform.position.y < - screenBounds.y) //Checks if projectile is visible by Camera
            Destroy(gameObject);
    }

}
