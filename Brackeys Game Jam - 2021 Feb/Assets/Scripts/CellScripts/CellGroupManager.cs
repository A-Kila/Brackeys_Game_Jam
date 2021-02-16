using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGroupManager : MonoBehaviour
{
    public Transform EmptyCellGroup;
    private Color currColor;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Divide()
    {
        Transform newGroup = Instantiate(EmptyCellGroup, transform.position, Quaternion.identity);
        int count = transform.childCount / 2;
        for (int i = 0; i < count; ++i)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(newGroup);
        }
        SelectHandler sh = FindObjectOfType<SelectHandler>();
        List<Color> colors = sh.colors;
        CellGroupManager newCellGroupManager = newGroup.GetComponent<CellGroupManager>();
        sh.dividedCellGroups.Add(newCellGroupManager);
        newCellGroupManager.EmptyCellGroup = EmptyCellGroup;
        newCellGroupManager.SelectGroup(colors[(sh.selectedCellGroups.Count + sh.dividedCellGroups.Count - 1) % colors.Count]);
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
