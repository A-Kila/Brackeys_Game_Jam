using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInPlace : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject target;
    private CellMovement cellMovement;
    private CellManager cellManager;
    private bool locked = false;
    private bool isQuitting = false;

    [HideInInspector]
    public Vector2 moveTowards;
    void Start()
    {
        cellMovement = GetComponent<CellMovement>();
        target = null;
        cellManager = GetComponent<CellManager>();
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }


    private void OnDestroy()
    {
        if (locked && !isQuitting) {
            if(target.GetComponent<VirusManager>() != null)
            target.GetComponent<VirusManager>().colliderCount--;
            else
                target.GetComponent<NeutralManager>().colliderCount--;
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(target != null) cellMovement.moveTowards = (Vector2)target.transform.position + moveTowards;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!locked && collision.gameObject == target && collision.gameObject.tag == "Enemy")
        {
            target.GetComponent<VirusManager>().colliderCount++;
            locked = true;
        }else if (!locked && collision.gameObject == target && collision.gameObject.tag == "Neutral")
        {
            target.GetComponent<NeutralManager>().colliderCount++;
            locked = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (locked && collision.gameObject == target && collision.gameObject.tag == "Enemy")
        {
            target.GetComponent<VirusManager>().colliderCount--;
            locked = false;
        }
        else if (!locked && collision.gameObject == target && collision.gameObject.tag == "Neutral")
        {
            target.GetComponent<NeutralManager>().colliderCount--;
            locked = false;
        }
    }

    public void selectTarget(GameObject target, float targetSpeed)
    {
        this.target = target;
        cellMovement.setTargetVisibility(false);
        cellMovement.SetSpeed(targetSpeed);
    }

    public void removeTarget()
    {
        cellMovement.SetSpeed(cellManager.speed);
        if (locked && target.GetComponent<VirusManager>() != null) target.GetComponent<VirusManager>().colliderCount--;
        else if(locked) target.GetComponent<NeutralManager>().colliderCount--;
        locked = false;
        target = null;
    }
}

/*
  if (!cellManager.selected) return;
      

        if (Input.GetKeyDown(MyInput.targetSelect))
        {
            if (target != null) target = null;
            else
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D[] targetCollider = Physics2D.OverlapAreaAll(mousePos, mousePos);

                if (targetCollider.Length != 0)
                    target = targetCollider[0].gameObject;
            }
        }
    }
 */
