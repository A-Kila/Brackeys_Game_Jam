using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField]
    private Transform Projectile;
    private Vector3 TargetLoc;
    private bool shoot = false;
    // Start is called before the first frame update
    public void setTarget(Vector3 targ)
    {
        TargetLoc = targ;
        shoot = true;
    }

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            creatProjectile();
            shoot = false;
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
