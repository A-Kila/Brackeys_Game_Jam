using UnityEngine;

public class HealthBuff : MonoBehaviour {
	
    public int healthAmount = 3;

    void Start() {
        GetComponent<BuffManager>().onCollide = AddHealthAction;
    }

    private void AddHealthAction(Transform objTransform) {
        objTransform.GetComponent<CellManager>().health.AddHealth(healthAmount);
    }

}
