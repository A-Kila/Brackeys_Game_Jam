using UnityEngine;

public class HealthBuff : MonoBehaviour {
	
    public int healthAmount = 3;

    void Start() {
        GetComponent<BuffManager>().onCollide = AddHealth;
    }

    private void AddHealth(Transform objTransform) {
        transform.GetComponent<CellManager>().health.AddHealth(healthAmount);
    }

}
