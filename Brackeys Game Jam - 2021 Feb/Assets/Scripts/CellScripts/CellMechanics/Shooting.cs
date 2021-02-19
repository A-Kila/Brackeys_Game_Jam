using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private CellManager cm;
    private ShootProjectile projectile;
    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponent<CellManager>();
        projectile = GetComponent<ShootProjectile>();
        cm.selectFuncs += onSelect;
        cm.deSelectFuncs += onDeselect;
        cm.stopActionsFuncs += stopShooting;
    }

    // Update is called once per frame
    void Update()
    {
        ShootOnMouseClick();
    }
    private void stopShooting()
    {
        GetComponent<ShootProjectile>().stopShooting();
        GetComponent<ShootProjectile>().deleteTarget();
    }

    private void onSelect()
    {
       GetComponent<ShootProjectile>().SetMarkerVisibility(true);
    }

    private void onDeselect()
    {
       GetComponent<ShootProjectile>().SetMarkerVisibility(false);
    }

    private void ShootOnMouseClick()
    {
        if (!cm.selected) return;
        if (Input.GetKeyDown(MyInput.targetSelect))
        { 

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] targetCollider = Physics2D.OverlapAreaAll(mousePos, mousePos);

            if (targetCollider.Length == 0)
                projectile.setTarget(new Vector3(mousePos.x, mousePos.y, 0));
            else if(targetCollider[0].gameObject.tag == "Enemy")
                projectile.setTarget(targetCollider[0].gameObject);

        }
        if(Input.GetKeyDown(MyInput.startShoot))
        {
            GetComponent<CellManager>().shooting = true;
            projectile.startShooting();
        }
        if (Input.GetKeyDown(MyInput.stopShoot))
        {
            GetComponent<CellManager>().shooting = false;
            projectile.stopShooting();
        }
    }
}
