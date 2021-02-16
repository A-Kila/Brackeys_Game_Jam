using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGroupManager : MonoBehaviour
{
    private Color currColor;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Merge(CellGroupManager other)
    {
        if (other == this) return;
        other.SelectGroup(currColor);
        for (int i = 0; i < other.transform.childCount;)
        {
            Transform child = other.transform.GetChild(i);
            child.SetParent(transform);
            child.GetComponent<ShootProjectile>().stopShooting();
            child.GetComponent<ShootProjectile>().deleteTarget();
        }
        Destroy(other.gameObject);

        for(int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<ShootProjectile>().stopShooting();
            transform.GetChild(i).GetComponent<ShootProjectile>().deleteTarget();
        }
    }

    public void SelectGroup(Color color)
    {
        currColor = color;
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<CellManager>().Select(color);
        }
    }
    public void DeselectGroup()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<CellManager>().Deselect();
        }
    }
}
