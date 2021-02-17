using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGroupManager : MonoBehaviour
{
    public Transform EmptyCellGroup;
    private Color currColor;
    private SelectHandler sh;

    [HideInInspector]
    public System.Action<Transform> stopActionFunc;

    private void Start()
    {
        sh = FindObjectOfType<SelectHandler>();
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
            CellManager cm = child.GetComponent<CellManager>();
            if (cm.stopActionsFuncs != null) cm.stopActionsFuncs();
        }
        Destroy(other.gameObject);

        for(int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            CellManager cm = child.GetComponent<CellManager>();
            if (cm.stopActionsFuncs != null) cm.stopActionsFuncs();
        }
    }

    public void SelectGroup(Color color)
    {
        currColor = color;
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            LockInPlace lip = child.GetComponent<LockInPlace>();
            if (lip != null) sh.selectedAntibodys.Add(lip);
           child.GetComponent<CellManager>().Select(color);
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
