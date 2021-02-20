using System.Collections.Generic;
using UnityEngine;

public class RTSController : MonoBehaviour {
	
    public float distanceBetweenCircles = 2f;

    private HashSet<CellGroupManager> selectedCellGroups;
    private List<LockInPlace> selectedAntibodys;
    private List<ExplosionHandler> selectedExplosiveCells;
    private Camera gameCamera;

    void Start() {
        gameCamera = Camera.main;
    }

    void Update() {
        moveControl();
        lockControl();
        explosiveCellControl();
    }

    private void explosiveCellControl()
    {
        selectedExplosiveCells = GetComponent<SelectHandler>().selectedExplosiveCells;
        if (selectedAntibodys == null) return;
        if (Input.GetKeyDown(MyInput.targetSelect))
        {
            GameObject target = null;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] targetCollider = Physics2D.OverlapAreaAll(mousePos, mousePos);
            if (targetCollider.Length != 0)
            {
                target = targetCollider[0].gameObject;
                if(target.tag == "Enemy")
                foreach(ExplosionHandler eh in selectedExplosiveCells)
                {
                 if (eh == null) continue;
                 eh.setTarget(target);
                }
            }
            else
            {
                foreach (ExplosionHandler eh in selectedExplosiveCells)
                {
                    if (eh == null) continue;
                    eh.setTarget(mousePos);
                }
            }
        }
    }
    private void lockControl()
    {
        selectedAntibodys = GetComponent<SelectHandler>().selectedAntibodys;
        if (selectedAntibodys == null) return;
        if (Input.GetKeyDown(MyInput.targetSelect))
        {
            GameObject target = null;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] targetCollider = Physics2D.OverlapAreaAll(mousePos, mousePos);

            if (targetCollider.Length != 0 && targetCollider[0].gameObject.tag != "Obstacle")
            {
                target = targetCollider[0].gameObject;
                float targetSpeed;
                if (target.tag == "Enemy") targetSpeed = target.GetComponent<VirusManager>().speed;
                else if (target.tag == "Neutral") targetSpeed = target.GetComponent<NeutralManager>().speed;
                else targetSpeed = target.GetComponent<CellManager>().speed;

                Vector2 center = target.transform.position;
                int numCells = selectedAntibodys.Count;
                float targetRad = target.transform.localScale.magnitude / 2;

                List<Vector2> positions = GetPositionCircleHelper(center, targetRad * 4.0f / 10.0f, numCells);
                int index = 0;
                foreach (LockInPlace lip in selectedAntibodys)
                {
                    if (lip == null) continue;
                    lip.selectTarget(target, targetSpeed);
                    lip.moveTowards = positions[index++] - (Vector2)target.transform.position;
                }

            }
            else
            {
                foreach(LockInPlace lip in selectedAntibodys)
                {
                    if (lip == null) continue;
                    lip.removeTarget();
                    lip.moveTowards = lip.gameObject.transform.position; 
                }
            }
        }
    }
    private void moveControl()
    {
        selectedCellGroups = GetComponent<SelectHandler>().selectedCellGroups;
        if (selectedCellGroups == null) return;
        if (Input.GetKeyDown(MyInput.move))
        {
            Vector2 posOnWorldMap = gameCamera.ScreenToWorldPoint(Input.mousePosition);

            int numCells = 0;
            foreach (CellGroupManager cellGroup in selectedCellGroups)
                if(cellGroup != null)numCells += cellGroup.transform.childCount;

            List<Vector2> positions = GetPositionCircle(posOnWorldMap, numCells, distanceBetweenCircles);

            int listIndex = 0;
            foreach (CellGroupManager cellGroup in selectedCellGroups)
            {
                if (cellGroup == null) continue;
                foreach (Transform child in cellGroup.transform)
                {
                    CellMovement childMovement = child.GetComponent<CellMovement>();
                    if(childMovement.targetObj != null) Destroy(childMovement.targetObj);
                    childMovement.targetObj = Instantiate(childMovement.target, positions[listIndex], Quaternion.identity).gameObject;
                    childMovement.MoveLocation(positions[listIndex]);

                    listIndex = (listIndex + 1) % positions.Count;
                }
            }
        }
    }

    private List<Vector2> GetPositionCircle(Vector2 startPosition, int numCells, float distanceBetween) {
        List<Vector2> result = new List<Vector2>();
        result.Add(startPosition);
        numCells--;
        float distance = distanceBetween;
        for (int i = 6; numCells > 0; i += 5) {
            result.AddRange(GetPositionCircleHelper(startPosition, distance, i));
            distance += distanceBetween;
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
