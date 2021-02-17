using System.Collections.Generic;
using UnityEngine;

public class RTSController : MonoBehaviour {
	
    public float distanceBetweenCircles = 1f;

    private HashSet<CellGroupManager> selectedCellGroups;
    private Camera gameCamera;

    void Start() {
        gameCamera = Camera.main;
    }

    void Update() {
        selectedCellGroups = GetComponent<SelectHandler>().selectedCellGroups;
        if (selectedCellGroups == null) return;
        
        if (Input.GetMouseButtonDown(1)) {
            Vector2 posOnWorldMap = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            
            int numCells = 0;
            foreach (CellGroupManager cellGroup in selectedCellGroups) 
                numCells += cellGroup.transform.childCount;

            List<Vector2> positions =  GetPositionCircle(posOnWorldMap, numCells);

            int listIndex = 0;
            foreach (CellGroupManager cellGroup in selectedCellGroups) {
                foreach (Transform child in cellGroup.transform) { 
                    CellMovement childMovement = child.GetComponent<CellMovement>();
                    childMovement.MoveLocation(positions[listIndex]);

                    listIndex = (listIndex + 1) % positions.Count;
                }
            }
        }
    }

    private List<Vector2> GetPositionCircle(Vector2 startPosition, int numCells) {
        List<Vector2> result = new List<Vector2>();
        result.Add(startPosition);
        numCells--;
        float distance = distanceBetweenCircles;
        for (int i = 6; numCells > 0; i += 5) {
            result.AddRange(GetPositionCircleHelper(startPosition, distance, i));
            distance += distanceBetweenCircles;
            numCells -= i;
        }
        return result;
    }

    private List<Vector2> GetPositionCircleHelper(Vector2 startPosition, float distance, int numPos) {
        List<Vector2> result = new List<Vector2>();
        for (int i = 0; i < numPos; i++) {
            float angle = (360f / numPos) * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * new Vector2(1, 0);
            Vector2 position = startPosition + direction * distance;
            result.Add(position);
        }
        return result;
    }

}
