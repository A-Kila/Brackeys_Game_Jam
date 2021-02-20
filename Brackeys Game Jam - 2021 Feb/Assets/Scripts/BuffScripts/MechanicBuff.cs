using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicBuff : MonoBehaviour
{
    public float Power = 2f;
    public ParticleSystem Particle;
    private ParticleSystem particleInstance;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BuffManager>().onCollide = IncreaseShootSpeed;
        particleInstance = Instantiate(Particle, transform);
        particleInstance.Play();

    }

    // Update is called once per frame
    private void IncreaseShootSpeed(Transform transformObj)
    {
        transformObj.GetComponentInParent<CellGroupManager>().applyMechanicBuff(Power);
    }
}
