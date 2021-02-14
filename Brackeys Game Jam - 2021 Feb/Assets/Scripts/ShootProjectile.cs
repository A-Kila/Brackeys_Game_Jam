using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public float delay = 1;

    [SerializeField]
    private Transform Projectile;
    [SerializeField]
    private Transform TargetMarker;

    private GameObject targetObject;
    private Vector3 TargetLoc;
    private bool shoot = false;
    private bool targetIsSet = false;
    private float shootTime;
    // Start is called before the first frame update
    public void setTarget(Vector3 targ)
    {
       if(targetObject != null) Destroy(targetObject);
        shoot = false;
        TargetLoc = targ;
        targetObject =  Instantiate(TargetMarker, targ, Quaternion.identity).gameObject;
        targetIsSet = true;
    }

    public void startShooting()
    { 
        if(!targetIsSet || shoot) return;
        shoot = true;
        shootTime = Time.time;
        targetObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void stopShooting()
    {
        if (!targetIsSet) return;
        shoot = false;
        targetObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot && shootTime < Time.time)
        {
            creatProjectile();
            shootTime = Time.time + delay;
        }
    }

    void creatProjectile()
    {
        Transform projTransform = Instantiate(Projectile, transform.position, Quaternion.identity);
        projTransform.GetComponent<MoveProjectile>().setup(aimDirection());
    }

    Vector3 aimDirection()
    {
        Vector3 dir = (TargetLoc - transform.position).normalized;
        return dir;
    }

}
