using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public float delay = 1;
    public float ProjectileScale = 1f;

    [SerializeField]
    private Transform Projectile;
    [SerializeField]
    private Transform TargetMarker;

    private Vector2 TargetLoc;
    private GameObject targetObject;
    private GameObject targetEntity;
    private Vector2[] shootDirecitons;
    private bool shoot = false;
    private bool targetIsSet = false;
    private bool directionIsSet = false;
    private bool targetIsEntity = false;
    private float shootTime;

    public void setTarget(Vector2 targ)
    {
        deleteTarget();
        shoot = false;
        TargetLoc = targ;
        targetObject =  Instantiate(TargetMarker, targ, Quaternion.identity).gameObject;
        targetIsSet = true;
        directionIsSet = false;
        targetIsEntity = false;
    }

    public void setTarget(GameObject entity)
    {
        deleteTarget();
        targetEntity = entity;
        shoot = false;
        targetIsSet = true;
        targetIsEntity = true;
        directionIsSet = false;
    }

    public void deleteTarget()
    {
        targetIsSet = false;
        targetIsEntity = false;
        if (targetObject != null) Destroy(targetObject);
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
        if (targetIsSet && !targetIsEntity) targetObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void stopShooting()
    {
        if (!(targetIsSet || directionIsSet)) return;
        shoot = false;
        if (targetIsSet && !targetIsEntity) targetObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot && shootTime < Time.time)
        {
            Shoot();
            shootTime = Time.time + delay;
        }
        if (targetIsEntity && targetEntity == null)
        {
            stopShooting();
            deleteTarget();
        }
    }

    public void SetMarkerVisibility(bool b)
    {
        if (targetObject != null) targetObject.SetActive(b);
    }


    private void Shoot() {
        if (targetIsSet && !targetEntity) { 
            Vector2 dir = (TargetLoc - (Vector2)transform.position).normalized;
            creatProjectile(dir);
        }else if(targetIsSet && targetIsEntity) {
            Vector2 dir = estDir();
            creatProjectile(dir);
        }else {
            foreach (Vector2 dir in shootDirecitons)
                creatProjectile(dir);
        }
    }

    private Vector2 estDir()
    {
        Vector2 difVector = (targetEntity.transform.position - transform.position);
        Vector2 targetDir = (targetEntity.GetComponent<CellMovement>().moveTowards - (Vector2)targetEntity.transform.position).normalized;
        float timePassed = difVector.x / (difVector.normalized.x * GetComponent<CellManager>().speed);
        Vector2 vecSum = difVector + targetDir*timePassed*
            Mathf.Pow(targetEntity.GetComponent<CellMovement>().speed, 2) * Time.deltaTime;
        
        vecSum = vecSum.normalized;
        return vecSum;
    }

    private void creatProjectile(Vector2 dir)
    {
        Transform projTransform = Instantiate(Projectile, transform.position, Quaternion.identity);
        projTransform.tag = transform.tag;
        projTransform.GetComponent<ProjectileManager>().setup(dir);
    }

}
