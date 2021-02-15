using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHandler : MonoBehaviour
{
    [HideInInspector]
    public List<CellManager> selectedCells;

    [SerializeField]
    private Transform SelectionArea;

    private Vector3 startPos;
    private GameObject currSelectionArea;
    private bool pressed;
    // Start is called before the first frame update
    void Start()
    {
        selectedCells = new List<CellManager>();
        pressed = false;
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
            Collider2D[] collarr =  Physics2D.OverlapAreaAll(startPos, currPos);

            foreach(CellManager cm in selectedCells) //deselects previously selected cells
            {
                cm.Deselect();
            }

            selectedCells.Clear();
            foreach (Collider2D col in collarr) 
            {
                CellManager cm = col.GetComponent<CellManager>();
                if(cm != null) //checks if object has CellManage(if it is a cell)
                {
                    cm.Select(); 
                    selectedCells.Add(cm);
                }
            }
        }
     
    }
}
