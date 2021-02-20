using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGroupManager : MonoBehaviour
{
    public enum cellType {cell, antibody, explosiveCell };

    public Transform EmptyCellGroup;
    public cellType type; 
    private Color currColor;
    private SelectHandler sh;
    private bool ImmortalityBuffIsActive = false;
    private float startTime, duration;

    [HideInInspector]
    public System.Action<Transform> stopActionFunc;

    private void Start()
    {
        sh = FindObjectOfType<SelectHandler>();
    }

    private void Update()
    {
        if (ImmortalityBuffIsActive && Time.time - startTime > duration) removeImmortality();
    }
    public void Divide()
    {
        Transform newGroup = Instantiate(EmptyCellGroup, transform.position, Quaternion.identity);
        newGroup.SetParent(transform.parent);
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
        newCellGroupManager.SelectGroup(colors[(sh.selectedCellGroups.Count + sh.dividedCellGroups.Count - 1) % colors.Count], true);
    }
    public void Merge(CellGroupManager other)
    {
        if (other == this) return;
        other.SelectGroup(currColor, true);
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

    public void applyMechanicBuff(float power)
    {
        
        for(int i = 0; i < transform.childCount; ++i)
        {
            if (type == cellType.cell)
            {
                ShootProjectile sp = transform.GetChild(i).GetComponent<ShootProjectile>();
                sp.delay /= power;
            }
            if (type == cellType.explosiveCell)
            {
                ExplosionHandler eh = transform.GetChild(i).GetComponent<ExplosionHandler>();
                eh.damage *= (int)power;
            }
            if(type == cellType.antibody)
            {
                LockInPlace lip = transform.GetChild(i).GetComponent<LockInPlace>();
                lip.strength *= (int)power;
            }
        }
    }

    public void applyImmortality(float time)
    {
        if (type == cellType.explosiveCell) return;
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<CellManager>().health.immortal = true;
        }
        InGameCanvasManager.InvokeOnBuff(time);
    }

    public void removeImmortality()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<CellManager>().health.immortal = false;
        }
    }

    public void SelectGroup(Color color, bool reselect)
    {
        currColor = color;
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            LockInPlace lip = child.GetComponent<LockInPlace>();
            ExplosionHandler eh = child.GetComponent<ExplosionHandler>();
            if (!reselect && lip != null) sh.selectedAntibodys.Add(lip);
            if (!reselect && eh != null) sh.selectedExplosiveCells.Add(eh);
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
