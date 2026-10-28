using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public HealthBar healthBar;
    private void Start()
    {
        healthBar.SetMaxHealth(health);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}
