using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    public int damage = 100;
    public float radiusOfExplosion = 5f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ProjectileManager>().beforeDestroy += onExplosion;
    }

    // Update is called once per frame

    private void onExplosion()
    {
        Vector2 currLocation = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(currLocation, radiusOfExplosion);
        ParticleSystem eParticle = Instantiate(explosionParticle, transform.position, Quaternion.identity);

        if (!eParticle.isPlaying) eParticle.Play();
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
            }
            if (nm != null)
            {
                nm.health.DamageHealth(damage);
            }
        }
    }
}
