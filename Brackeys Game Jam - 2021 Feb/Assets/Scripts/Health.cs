using UnityEngine;

public class Health {
	
	public System.Action onPlayerDeath;

	private int maxHealth;
	public int health;

	public Health(int health) {
		maxHealth = health;
		this.health = maxHealth;
    }

	public void DamageHealth(int amount) {
		health -= amount;
		health = Mathf.Clamp(health, 0, maxHealth);

		if (health <= 0 && onPlayerDeath != null)
			onPlayerDeath();
	}

	public void AddHealth(int amount) {
		health += amount;
		health = Mathf.Clamp(health, 0, maxHealth);
    }

}
