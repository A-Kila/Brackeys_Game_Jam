using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralGroupManager : MonoBehaviour
{
    public Transform path;
    public float distanceBetweenCircles = 1f;


    [HideInInspector]
    public int waypointIndex = 0;
    [HideInInspector]
    public Vector2[] waypoints;

    void OnDrawGizmos()
    {
        Vector2 prevPos = path.GetChild(0).position;

        foreach (Transform waypoint in path)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(prevPos, waypoint.position);
            prevPos = waypoint.position;
        }

        Gizmos.DrawLine(prevPos, path.GetChild(0).position);
    }

    private Vector2[,] moveLocations;

    // Start is called before the first frame update
    void Start()
    {
        moveLocations = new Vector2[path.childCount, transform.childCount - 1];
        waypoints = new Vector2[path.childCount];
        for (int i = 0; i < path.childCount; ++i)
        {
            waypoints[i] = path.GetChild(i).position;
            List<Vector2> mc = moveControl(waypoints[i]);
            for (int j = 0; j < transform.childCount - 1; ++j)
            {
                moveLocations[i, j] = mc[j];
            }
        }
        
    }

    public Vector2 getNextPoint(int waypoint, int child)
    {
        return moveLocations[waypoint, child];
    }

    private List<Vector2> moveControl(Vector2 posOnWorldMap)
    {

        int numCells = transform.childCount - 1;
        List<Vector2> positions = GetPositionCircle(posOnWorldMap, numCells, distanceBetweenCircles);
        return positions;
         
    }

    private List<Vector2> GetPositionCircle(Vector2 startPosition, int numCells, float distanceBetween)
    {
        List<Vector2> result = new List<Vector2>();
        result.Add(startPosition);
        numCells--;
        float distance = distanceBetween;
        for (int i = 6; numCells > 0; i += 5)
        {
            result.AddRange(GetPositionCircleHelper(startPosition, distance, i));
            distance += distanceBetween;
            numCells -= i;
        }
        return result;
    }

    private List<Vector2> GetPositionCircleHelper(Vector2 startPosition, float distance, int numPos)
    {
        List<Vector2> result = new List<Vector2>();
        for (int i = 0; i < numPos; i++)
        {
            float angle = (360f / numPos) * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * new Vector2(1, 0);
            Vector2 position = startPosition + direction * distance;
            result.Add(position);
        }
        return result;
    }


}
