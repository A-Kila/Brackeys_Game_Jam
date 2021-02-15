﻿using UnityEngine;

public class CellManager : MonoBehaviour {
	
    public float speed = 15f;
    public int healthAmount = 100;

    private CellMovement movement;
    private Camera gameCamera;
    private Health health;

    void Start() {
        gameCamera = FindObjectOfType<Camera>();

        movement = GetComponent<CellMovement>();
        movement.SetSpeed(speed);

        health = new Health(healthAmount);
        health.onPlayerDeath += PlayerDeath;
    }

    void Update() {
        MoveCell();
        ShootOnMouseClick();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        int projectileDamage = collider.GetComponent<ProjectileManager>().damage;
        if (collider.tag == "Enemy") { 
            health.DamageHealth(projectileDamage);
            // Animation
            Destroy(collider.gameObject);
        }
        // if (collider.tag == powerup) {
        //     health.Addhealth(healthAmount);
        //     Animation
        //     Destroy(collider.gameObject);
        // }
    }

    private void ShootOnMouseClick() {
        if (Input.GetMouseButtonDown(1)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.GetComponent<ShootProjectile>().setTarget(new Vector3(mousePos.x, mousePos.y, 0));
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            gameObject.GetComponent<ShootProjectile>().startShooting();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            gameObject.GetComponent<ShootProjectile>().stopShooting();
        }
    }

    private void MoveCell() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 posOnWorldMap = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            movement.MoveLocation(posOnWorldMap);
        }
    }

    private void PlayerDeath() {
        Destroy(gameObject);
        // Animation
        health.onPlayerDeath -= PlayerDeath;
    }
}
