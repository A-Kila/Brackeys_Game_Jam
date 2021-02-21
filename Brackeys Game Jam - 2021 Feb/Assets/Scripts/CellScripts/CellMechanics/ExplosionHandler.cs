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
    private Vector2 targPlace;
    private bool targPlaceIsSet = false;

    public Transform targetMarker;
    private GameObject targetMarkerObj;

    // Start is called before the first frame update
    void Start()
    {
        CellManager cm = GetComponent<CellManager>();
        cm.onPlayerDeathFuncs += onExplosion;
        cm.onPlayerDeathFuncs += destroyTarget;
        cm.selectFuncs += onSelect;
        cm.deSelectFuncs += onDeselect;
        cellMovement = GetComponent<CellMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) cellMovement.moveTowards = (Vector2)target.transform.position;
        if (targPlaceIsSet) cellMovement.moveTowards = targPlace;
        if(targPlaceIsSet && targPlace == (Vector2)transform.position) GetComponent<CellManager>().PlayerDeath();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        GetComponent<CellManager>().PlayerDeath();
    }

    public void setTarget(GameObject gObj)
    {
        target = gObj;
        ParticleSystem sParticle = Instantiate(speedParticle, transform);
        sParticle.Play();
        FindObjectOfType<AudioManager>().Play("ExplosiveAcceleration");
        cellMovement.SetSpeed(GetComponent<CellManager>().speed * speedMultiply);
        target.GetComponent<VirusManager>().HighlightSet(true);
    }

    public void setTarget(Vector2 targ)
    {
        targPlace = targ;
        ParticleSystem sParticle = Instantiate(speedParticle, transform);
        sParticle.Play();
        FindObjectOfType<AudioManager>().Play("ExplosiveAcceleration");
        cellMovement.SetSpeed(GetComponent<CellManager>().speed * speedMultiply);
        targetMarkerObj = Instantiate(targetMarker, targ, Quaternion.identity).gameObject;
        targPlaceIsSet = true;
    }

    private void onSelect()
    {
        setTargetVisibility(true);
    }
    private void onDeselect()
    {
        setTargetVisibility(false);
    }
    private void setTargetVisibility(bool b)
    {
        if (targPlaceIsSet = true && targetMarkerObj != null) targetMarkerObj.SetActive(b);
        else if (target != null) target.GetComponent<VirusManager>().HighlightSet(b);
    }
    private void destroyTarget()
    {
        if (targPlaceIsSet = true && targetMarkerObj != null)
        {
            targetMarkerObj.SetActive(false);
            Destroy(targetMarkerObj);
        }
        else if (target != null) target.GetComponent<VirusManager>().HighlightSet(false);
    }
    private void onExplosion()
    {
        Vector2 currLocation = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(currLocation, radiusOfExplosion);
        ParticleSystem eParticle = Instantiate(explosionParticle, transform.position, Quaternion.identity);

       if(!eParticle.isPlaying) eParticle.Play();
        Destroy(eParticle.gameObject, eParticle.main.duration);

        FindObjectOfType<AudioManager>().Play("CellExplosion");


        foreach (Collider2D collider in colliders)
        {
            CellManager cm = collider.gameObject.GetComponent<CellManager>();
            VirusManager vm = collider.gameObject.GetComponent<VirusManager>();
            NeutralManager nm = collider.gameObject.GetComponent<NeutralManager>();
            if (cm != null)
            {
                cm.health.DamageHealth(damage);
            }
            if (vm != null)
            {
                vm.health.DamageHealth(damage);
                GameHandler.money += damage/3;
            }
            if(nm != null)
            {
                nm.health.DamageHealth(damage);
                GameHandler.money += damage/3;
            }
        }
    }
}
