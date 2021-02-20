using UnityEngine;

public class Health {
	
	public System.Action onPlayerDeath;
	public HealthBarControler healthBar;

	private int maxHealth;
	public int health;

	public Health(int health, HealthBarControler healthBar) {
		this.healthBar = healthBar;
		maxHealth = health;
		this.health = maxHealth;
		healthBar.SetMaxHealth(health);
    }

	public Health(int health)
    {
		maxHealth = health;
		this.health = maxHealth;
	}

	public void DamageHealth(int amount) {
		health -= amount;
		health = Mathf.Clamp(health, 0, maxHealth);
		if(healthBar != null)healthBar.SetHealth(health);

		if (health <= 0 && onPlayerDeath != null)
			onPlayerDeath();
	}

	public void AddHealth(int amount) {
		health += amount;
		health = Mathf.Clamp(health, 0, maxHealth);
		if(healthBar != null)healthBar.SetHealth(health);
	}

}
