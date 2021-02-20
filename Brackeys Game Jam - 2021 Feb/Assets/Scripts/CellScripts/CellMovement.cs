using UnityEngine;

public class CellMovement : MonoBehaviour {

    [HideInInspector]
    public float speed;
    private Rigidbody2D rb;

    [HideInInspector]
    public Vector2 moveTowards;
    private float halfWidth, halfHeight, halfRBRadius;

    public Transform target;
    [HideInInspector]
    public GameObject targetObj;
    [HideInInspector]
    public bool selecter = false;
    private CellManager cellManager;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        moveTowards = transform.position;
        cellManager = GetComponent<CellManager>();
        if (cellManager != null) cellManager.onPlayerDeathFuncs += destroyTarget;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
        halfRBRadius = rb.transform.localScale.x / 2;   
    }

    void FixedUpdate() {
        rb.MovePosition(Vector3.MoveTowards(rb.position, moveTowards, speed * Time.deltaTime));
        if (rb.position == moveTowards) destroyTarget();
    }

    public void setTargetVisibility(bool b)
    {
        if (targetObj != null) targetObj.SetActive(b);
    }
    public void destroyTarget()
    {
        if (targetObj != null) Destroy(targetObj);
    }

    public void MoveLocation(Vector2 moveLocation) {
        moveLocation.x = Mathf.Clamp(moveLocation.x, -halfWidth + halfRBRadius, halfWidth - halfRBRadius);
        moveLocation.y = Mathf.Clamp(moveLocation.y, -halfHeight + halfRBRadius, halfHeight - halfRBRadius);
        moveTowards = moveLocation;
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

}
