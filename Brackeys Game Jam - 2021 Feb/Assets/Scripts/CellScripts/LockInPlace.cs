using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInPlace : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject target;
    private CellMovement cellMovement;
    private CellManager cellManager;
    void Start()
    {
        cellMovement = GetComponent<CellMovement>();
        cellManager = GetComponent<CellManager>();
        target = null;
    }


    // Update is called once per frame
    void Update()
    {
       if(target != null) cellMovement.moveTowards = target.transform.position;
        selectTarget();
    }

    private void selectTarget()
    {
        if (!cellManager.selected) return;

        if (Input.GetKeyDown(MyInput.targetSelect))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] targetCollider = Physics2D.OverlapAreaAll(mousePos, mousePos);

            if (targetCollider.Length != 0)
                target = targetCollider[0].gameObject;
        }
    }
}
