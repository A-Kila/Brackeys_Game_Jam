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
            Vector2 dir = estDir().normalized;
            creatProjectile(dir);
        }else {
            foreach (Vector2 dir in shootDirecitons)
                creatProjectile(dir);
        }
    }


    private Vector2 estDir()
    {
        Vector2 difPos = (targetEntity.transform.position - transform.position);
        Vector2 enemyDir = (targetEntity.GetComponent<CellMovement>().moveTowards - (Vector2)targetEntity.transform.position).normalized;
        Vector2 thisDir = (GetComponent<CellMovement>().moveTowards - (Vector2)transform.position).normalized;
        Vector2 difVel = enemyDir*targetEntity.GetComponent<CellMovement>().speed
            - thisDir*GetComponent<CellMovement>().speed;

        float a = difVel.SqrMagnitude() - Mathf.Pow(Projectile.gameObject.GetComponent<ProjectileManager>().speed, 2);
        float b = 2f*Vector2.Dot(difVel, difPos);
        float c = difPos.SqrMagnitude();

        float det = Mathf.Pow(b, 2) - 4f * a * c;

        float time = 0;

        if (det < 0)
        {
           time = Mathf.Abs(-b/(2f * a));

        }
        else
        {
            float sqrt = Mathf.Sqrt(det);
            float x1 = (-b - sqrt) / (2f * a);
            time = x1;

        }
        Vector2 dir = (Vector2)targetEntity.transform.position + enemyDir * time * targetEntity.GetComponent<CellMovement>().speed - (Vector2)transform.position;
        return dir;
    }


    private void creatProjectile(Vector2 dir)
    {
        Transform projTransform = Instantiate(Projectile, transform.position, Quaternion.identity);
        projTransform.tag = transform.tag;
        projTransform.GetComponent<ProjectileManager>().setup(dir);
    }

}
