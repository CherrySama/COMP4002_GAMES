using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat strength;
    public Stat damage;
    public Stat maxHealth;

    [SerializeField] private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStat)
    {
        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStat.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(damage);

        if (currentHealth < 0)
            Die();
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died!");
        //throw new NotImplementedException();
    }
}
