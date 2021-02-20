using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalityBuff : MonoBehaviour
{
    public float Time = 10f;
    public ParticleSystem Particle;
    private ParticleSystem particleInstance;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BuffManager>().onCollide = makeImmortal;
        particleInstance = Instantiate(Particle, transform);
        particleInstance.Play();

    }

    // Update is called once per frame
    private void makeImmortal(Transform transformObj)
    {
        transformObj.GetComponentInParent<CellGroupManager>().applyImmortality(Time);
    }
}
