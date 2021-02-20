using UnityEngine;

public class ObstacleManager : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider) {
        ProjectileManager projectile = collider.GetComponent<ProjectileManager>();
        if (projectile != null) projectile.DestroyProjectile();
        else Destroy(collider.gameObject);
    }

}
