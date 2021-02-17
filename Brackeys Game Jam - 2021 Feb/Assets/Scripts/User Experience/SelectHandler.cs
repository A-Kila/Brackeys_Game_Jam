using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHandler : MonoBehaviour
{
    [HideInInspector]
    public HashSet<CellGroupManager> selectedCellGroups;
    [HideInInspector]
    public List<LockInPlace> selectedAntibodys;
    [HideInInspector]
    public HashSet<CellGroupManager> dividedCellGroups;
    [HideInInspector]
    public List<Color> colors;

    [SerializeField]
    private Transform SelectionArea;

    private Vector3 startPos;
    private GameObject currSelectionArea;
    private bool pressed;
    // Start is called before the first frame update
    void Start()
    {
        selectedCellGroups = new HashSet<CellGroupManager>();
        dividedCellGroups = new HashSet<CellGroupManager>();
        pressed = false;
        colors = new List<Color> { Color.red, Color.yellow, Color.cyan, Color.blue, Color.grey, Color.black, Color.white };
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(MyInput.select))
        {
            startPos = currPos;
            startPos.z = 0;
            currSelectionArea = Instantiate(SelectionArea, startPos, Quaternion.identity).gameObject;
            pressed = true;
        }
        if (pressed)
        {
            currPos.z = 0;
            Vector3 size =  currPos - startPos;
            currSelectionArea.transform.localScale = size;
        }

        if (Input.GetKeyUp(MyInput.select))
        {
            Destroy(currSelectionArea);
            pressed = false;
            Collider2D[] colliders =  Physics2D.OverlapAreaAll(startPos, currPos);

            foreach(CellGroupManager cgm in selectedCellGroups) //deselects previously selected cells
            {
                if (cgm == null) continue;
                cgm.DeselectGroup();
            }

            selectedAntibodys.Clear();
            selectedCellGroups.Clear();

            foreach (Collider2D col in colliders) 
            {
                CellManager cm = col.GetComponent<CellManager>();
                LockInPlace lip = col.GetComponent<LockInPlace>();
                if(cm != null) //checks if object has CellManage(if it is a cell)
                {
                    CellGroupManager groupManager = cm.GetComponent<Transform>().GetComponentInParent<CellGroupManager>();
                    selectedCellGroups.Add(groupManager);
                }
                if (lip != null) selectedAntibodys.Add(lip);
            }
            int groupNum = 0;
            foreach(CellGroupManager cgm in selectedCellGroups)
            {
                cgm.SelectGroup(colors[groupNum++ % colors.Count]); //seting different color for different groups
            }
        }
        if (Input.GetKeyDown(MyInput.combineGroups)) combineSelectedGroups();
        if (Input.GetKeyDown(MyInput.divideGroups)) divideSelectedGroups();
    }
    private void combineSelectedGroups()
    {
        if (selectedCellGroups.Count < 2) return;
        CellGroupManager firstGroup = null;
        foreach(CellGroupManager cgm in selectedCellGroups)
        {
            if (cgm == null) continue;
            if (firstGroup == null) firstGroup = cgm;
            else firstGroup.Merge(cgm);
        }
        selectedCellGroups.Clear();
        selectedCellGroups.Add(firstGroup);
    }

    private void divideSelectedGroups()
    {
        foreach (CellGroupManager cgm in selectedCellGroups)
        {
            if (cgm == null) continue;
            if (cgm.GetComponent<Transform>().childCount < 2) continue;
            cgm.Divide();
        }
        foreach (CellGroupManager cgm in dividedCellGroups)
            selectedCellGroups.Add(cgm);
        dividedCellGroups.Clear();
    }
}
