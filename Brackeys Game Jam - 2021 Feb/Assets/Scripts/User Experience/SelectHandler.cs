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
    public List<ExplosionHandler> selectedExplosiveCells;
    [HideInInspector]
    public HashSet<CellGroupManager> dividedCellGroups;
    [HideInInspector]
    public List<Color> colors;

    [SerializeField]
    private Transform SelectionArea;

    private Vector3 startPos;
    private GameObject currSelectionArea;
    private bool pressed;
    private GameObject mouseClickedObj;
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
            if (pressed == false) return;
            deSelect();
            select(currPos);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] targetCollider = Physics2D.OverlapAreaAll(mousePos, mousePos);

            if (targetCollider.Length != 0)
            {
                if (mouseClickedObj == targetCollider[0].gameObject) selectAllOfType(mouseClickedObj.transform.parent.GetComponent<CellGroupManager>().type);
                else mouseClickedObj = targetCollider[0].gameObject;
            }
            
        }
        else if(Input.anyKeyDown) mouseClickedObj = null;
        

        if (Input.GetKeyDown(MyInput.combineGroups)) combineSelectedGroups();
        if (Input.GetKeyDown(MyInput.divideGroups)) divideSelectedGroups();
    }
    private void combineSelectedGroups()
    {
        if (selectedCellGroups.Count < 2) return;
        CellGroupManager firstTypeFirstGroup = null;
        CellGroupManager secondTypeFirstGroup = null;
        foreach (CellGroupManager cgm in selectedCellGroups)
        {
            if (cgm == null) continue;
            if (firstTypeFirstGroup == null && cgm.type == CellGroupManager.cellType.cell) firstTypeFirstGroup = cgm;
            else if(cgm.type == CellGroupManager.cellType.cell) firstTypeFirstGroup.Merge(cgm);

            if (secondTypeFirstGroup == null && cgm.type == CellGroupManager.cellType.antibody) secondTypeFirstGroup = cgm;
            else if (cgm.type == CellGroupManager.cellType.antibody) secondTypeFirstGroup.Merge(cgm);
        }
        selectedCellGroups.Clear();
       if (firstTypeFirstGroup != null) selectedCellGroups.Add(firstTypeFirstGroup);
       if (secondTypeFirstGroup != null) selectedCellGroups.Add(secondTypeFirstGroup);
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

    private void selectAllOfType(CellGroupManager.cellType type)
    {
        deSelect();
        int groupNum = 0;
        foreach (CellGroupManager cgm in FindObjectsOfType<CellGroupManager>())
        {

            if (cgm.type == type)
            {
                selectedCellGroups.Add(cgm);
                Debug.Log(groupNum);
                cgm.SelectGroup(colors[groupNum++ % colors.Count], false);
            }
        }
        pressed = false;
    }

    private void deSelect()
    {
        foreach (CellGroupManager cgm in selectedCellGroups) //deselects previously selected cells
        {
            if (cgm == null) continue;
            cgm.DeselectGroup();
        }

        selectedAntibodys.Clear();
        selectedExplosiveCells.Clear();
        selectedCellGroups.Clear();
    }
    private void select(Vector2 currPos)
    {
        Destroy(currSelectionArea);
        pressed = false;
        Collider2D[] colliders = Physics2D.OverlapAreaAll(startPos, currPos);

        foreach (Collider2D col in colliders)
        {
            CellManager cm = col.GetComponent<CellManager>();
            if (cm != null) //checks if object has CellManage(if it is a cell)
            {
                CellGroupManager groupManager = cm.GetComponent<Transform>().GetComponentInParent<CellGroupManager>();
                selectedCellGroups.Add(groupManager);
            }
        }
        int groupNum = 0;
        foreach (CellGroupManager cgm in selectedCellGroups)
        {
            cgm.SelectGroup(colors[groupNum++ % colors.Count], false); //seting different color for different groups
        }
    }
}
