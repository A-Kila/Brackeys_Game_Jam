using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public float delay = 1;

    [SerializeField]
    private Transform Projectile;
    [SerializeField]
    private Transform TargetMarker;

    private Vector2 TargetLoc;
    private CellMovement cellMovement;
    private GameObject targetObject;
    private GameObject targetEntity;
    private Vector2[] shootDirecitons;
    private bool shoot = false;
    private bool targetIsSet = false;
    private bool directionIsSet = false;
    private bool targetIsEntity = false;
    private float shootTime;

    private void Start()
    {
        cellMovement = GetComponent<CellMovement>();
    }
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
        if (targetIsSet && !targetIsEntity) {
            Vector2 dir = (TargetLoc - (Vector2)transform.position).normalized;
            creatProjectile(dir);
        }else if(targetIsSet && targetIsEntity) {
            Vector2 dir = (estDir(transform.position, targetEntity.transform.position,
                targetEntity.GetComponent<CellMovement>().moveTowards, 
                targetEntity.GetComponent<CellMovement>().speed, Projectile.gameObject.GetComponent<ProjectileManager>().speed, 0) - (Vector2)transform.position).normalized;
            creatProjectile(dir);
        }else {
            foreach (Vector2 dir in shootDirecitons) {
                creatProjectile(dir);
            }
        }
    }


    private Vector2 estDir(Vector2 thisPos, Vector2 enemyPos, Vector2 enemyDes, float enemySpeed, float thisSpeed, float disFromThis)
    {
        Vector2 difPos = (enemyPos - thisPos);
        Vector2 enemyDir = (enemyDes - enemyPos).normalized;
        Vector2 difVel = enemyDir* enemySpeed;

        float a = difVel.SqrMagnitude() - Mathf.Pow(thisSpeed, 2);
        float b = 2f*Vector2.Dot(difVel, difPos) - 2f*thisSpeed* disFromThis;
        float c = difPos.SqrMagnitude() - Mathf.Pow(disFromThis, 2);

        float det = Mathf.Pow(b, 2) - 4f * a * c;

        float time = 0;

        if (det < 0.1f)
        {
            time = Mathf.Abs(-b/(2f * a));
        }
        else
        {
            float sqrt = Mathf.Sqrt(det);
            float x1 = (-b - sqrt) / (2f * a);
            float x2 = (-b + sqrt) / (2f * a);
            if(x2 < 0.1f)
            {
                if (x1 < 0.1f)
                {
                    time = Mathf.Abs(-b / (2f * a));
                }
                else time = x1;
            }
            else time = Mathf.Min(x1, x2);

        }
        Vector2 estEnemyLoc = enemyPos + enemyDir * time * enemySpeed;
        VirusManager vm = targetEntity.GetComponent<VirusManager>();
        if ((enemyDes - enemyPos).magnitude < (estEnemyLoc - enemyPos).magnitude)
        {
            float timePassed = (enemyDes - enemyPos).magnitude / enemySpeed;
            estEnemyLoc = estDir(thisPos, enemyDes, vm.waypoints[(vm.waypointIndex + 1) % vm.waypoints.Length], enemySpeed, thisSpeed, thisSpeed*timePassed);
        }
        return estEnemyLoc;
    }


    private void creatProjectile(Vector2 dir)
    {
        Transform projTransform = Instantiate(Projectile, (Vector2)transform.position, Quaternion.identity);
        projTransform.tag = transform.tag;

        ProjectileManager  manager = projTransform.GetComponent<ProjectileManager>();
        manager.parentObj = gameObject;
        manager.setup(dir);
    }

}
