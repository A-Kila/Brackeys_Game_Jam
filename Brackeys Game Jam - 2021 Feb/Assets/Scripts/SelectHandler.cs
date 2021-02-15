using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHandler : MonoBehaviour
{
    [HideInInspector]
    public HashSet<CellGroupManager> selectedCellGroups;

    [SerializeField]
    private Transform SelectionArea;

    private List<Color> colors;
    private Vector3 startPos;
    private GameObject currSelectionArea;
    private bool pressed;
    // Start is called before the first frame update
    void Start()
    {
        selectedCellGroups = new HashSet<CellGroupManager>();
        pressed = false;
        colors = new List<Color> { Color.red, Color.yellow, Color.cyan };
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
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

        if (Input.GetMouseButtonUp(0))
        {
            Destroy(currSelectionArea);
            pressed = false;
            Collider2D[] colliders =  Physics2D.OverlapAreaAll(startPos, currPos);

            foreach(CellGroupManager cgm in selectedCellGroups) //deselects previously selected cells
            {
                cgm.DeselectGroup();
            }

            selectedCellGroups.Clear();
            foreach (Collider2D col in colliders) 
            {
                CellManager cm = col.GetComponent<CellManager>();
                if(cm != null) //checks if object has CellManage(if it is a cell)
                {
                    CellGroupManager groupManager = cm.GetComponent<Transform>().GetComponentInParent<CellGroupManager>();
                    selectedCellGroups.Add(groupManager);
                }
            }
            int groupNum = 0;
            foreach(CellGroupManager cgm in selectedCellGroups)
            {
                cgm.SelectGroup(colors[groupNum++ % colors.Count]); //seting different color for different groups
            }
        }
        if (Input.GetKeyDown(KeyCode.E)) combineSelectedGroups();
    }
    private void combineSelectedGroups()
    {
        if (selectedCellGroups.Count < 2) return;
        CellGroupManager firstGroup = null;
        foreach(CellGroupManager cgm in selectedCellGroups)
        {
            if (firstGroup == null) firstGroup = cgm;
            else firstGroup.Merge(cgm);
        }
        selectedCellGroups.Clear();
        selectedCellGroups.Add(firstGroup);
    }
}
