using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    public int damage = 100;
    public float radiusOfExplosion = 5f;
    public float speedMultiply = 2f;
    public ParticleSystem explosionParticle;
    public ParticleSystem speedParticle;

    private GameObject target;
    private CellMovement cellMovement;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CellManager>().onPlayerDeathFuncs += onExplosion;
        cellMovement = GetComponent<CellMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) cellMovement.moveTowards = (Vector2)target.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag != "Friendly")
        GetComponent<CellManager>().PlayerDeath();
    }

    public void setTarget(GameObject gObj)
    {
        target = gObj;
        ParticleSystem sParticle = Instantiate(speedParticle, transform);
        sParticle.Play();
        cellMovement.SetSpeed(GetComponent<CellManager>().speed * speedMultiply);
    }

    private void onExplosion()
    {
        Vector2 currLocation = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(currLocation, 5);
        ParticleSystem eParticle = Instantiate(explosionParticle, transform.position, Quaternion.identity);

       if(!eParticle.isPlaying) eParticle.Play();
        Destroy(eParticle, eParticle.main.duration);


        foreach (Collider2D collider in colliders)
        {
            CellManager cm = collider.gameObject.GetComponent<CellManager>();
            VirusManager vm = collider.gameObject.GetComponent<VirusManager>();
            if (cm != null)
            {
                cm.health.DamageHealth(damage);
            }
            if (vm != null)
            {
                vm.health.DamageHealth(damage);
            }
        }
    }
}
