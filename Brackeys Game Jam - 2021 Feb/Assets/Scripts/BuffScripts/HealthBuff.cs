using UnityEngine;

public class HealthBuff : MonoBehaviour {

    public ParticleSystem Particle;
    public int healthAmount = 3;
    private ParticleSystem particleInstance;

    void Start() {
        GetComponent<BuffManager>().onCollide = AddHealthAction;
        particleInstance = Instantiate(Particle, transform);
        particleInstance.Play();
    }

    private void AddHealthAction(Transform objTransform) {
        objTransform.GetComponent<CellManager>().health.AddHealth(healthAmount);
    }

}
