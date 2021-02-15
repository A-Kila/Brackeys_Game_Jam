using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public float delay = 1;

    [SerializeField]
    private Transform Projectile;
    [SerializeField]
    private Transform TargetMarker;

    private GameObject targetObject;
    private Vector2 TargetLoc;
    private Vector2[] shootDirecitons;
    private bool shoot = false;
    private bool targetIsSet = false;
    private bool directionIsSet = false;
    private float shootTime;

    public void setTarget(Vector2 targ)
    {
        if(targetObject != null) Destroy(targetObject);
        shoot = false;
        TargetLoc = targ;
        targetObject =  Instantiate(TargetMarker, targ, Quaternion.identity).gameObject;
        targetIsSet = true;
        directionIsSet = false;
    }

    public void setDirections(Vector2[] dir)
    {
        if(targetObject != null) Destroy(targetObject);
        shoot = false;
        shootDirecitons = dir;
        targetIsSet = false;
        directionIsSet = true;
    }

    public void startShooting()
    { 
        if(!(targetIsSet || directionIsSet) || shoot) return;
        shoot = true;
        shootTime = Time.time;
        if (targetIsSet) targetObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void stopShooting()
    {
        if (!(targetIsSet || directionIsSet)) return;
        shoot = false;
        if (targetIsSet) targetObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot && shootTime < Time.time)
        {
            Shoot();
            shootTime = Time.time + delay;
        }
    }

    void Shoot() {
        if (targetIsSet) { 
            Vector2 dir = (TargetLoc - (Vector2)transform.position).normalized;
            creatProjectile(dir);
        } else {
            foreach (Vector2 dir in shootDirecitons)
                creatProjectile(dir);
        }
    }

    void creatProjectile(Vector2 dir)
    {
        Transform projTransform = Instantiate(Projectile, transform.position, Quaternion.identity);
        projTransform.tag = transform.tag;
        projTransform.GetComponent<ProjectileManager>().setup(dir);
    }

}
