using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int damage;
    public int maxHealth;

    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
    }
}
