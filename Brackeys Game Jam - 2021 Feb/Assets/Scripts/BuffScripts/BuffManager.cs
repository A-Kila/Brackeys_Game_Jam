using UnityEngine;

public class BuffManager : MonoBehaviour {
	
    public System.Action<Transform> onCollide;

    public void onCollideDoAction(Transform objTransform) {
        if (onCollide != null)
            onCollide(objTransform);
    } 

}
