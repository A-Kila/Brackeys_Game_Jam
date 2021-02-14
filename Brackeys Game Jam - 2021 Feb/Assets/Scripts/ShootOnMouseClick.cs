using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOnMouseClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.GetComponent<ShootProjectile>().setTarget(new Vector3(mousePos.x, mousePos.y, 0));
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            gameObject.GetComponent<ShootProjectile>().startShooting();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            gameObject.GetComponent<ShootProjectile>().stopShooting();
        }
    }
}
