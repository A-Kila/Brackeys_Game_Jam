using System.Collections;
using UnityEngine;

public class ShopManger : MonoBehaviour {
	
    [SerializeField]
    public Transform spawnPoint;
    [SerializeField]
    private Transform regularCell;
    [SerializeField]
    private Transform explosiveCell;
    [SerializeField]
    private Transform antibodyCell;

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(spawnPoint.position, .3f);
    }

    public void BuyRegular() {
        if (GameHandler.money < 5) return;
        GameHandler.money -= 5;
        Instantiate(regularCell, spawnPoint.position, Quaternion.identity, GameObject.FindGameObjectWithTag("CellGroupHolder").transform);
    }

    public void BuyExplosive() {
        if (GameHandler.money < 10) return;
        GameHandler.money -= 10;
        Instantiate(explosiveCell, spawnPoint.position, Quaternion.identity, GameObject.FindGameObjectWithTag("CellGroupHolder").transform);
    }

    public void BuyAntibody() {
        if (GameHandler.money < 15) return;
        GameHandler.money -= 15;
        Instantiate(antibodyCell, spawnPoint.position, Quaternion.identity, GameObject.FindGameObjectWithTag("CellGroupHolder").transform);
    }

}
