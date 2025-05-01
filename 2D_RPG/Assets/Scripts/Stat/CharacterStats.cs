using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength;      // 1 point increase damage by 1 
    public Stat aglity;        // 1 point increase evasion by 1%
    public Stat intelligence;  // 1 point increase magic damage by 1 
    public Stat vitality;      // 1 point increase maxhealth by 5
    public Stat element;       // 0 -> normal, 1 -> fire, 2 -> ice

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Offensive Stat")]
    public Stat damage;
    public Stat fireDamage;
    public Stat iceDamage;

    [Header("EXP Stat")]
    public Stat lv;            // 1 lv up -> get 1 point
    public Stat experience_point; // 5 to 1 lv
    public Stat attri_points; // 1 lv up get 1 point 
    public Stat killed_num; // kill 1 get 1 exp

    public bool isChilled; // do damage over time or 1 more damage
    public bool isIgnited; // reduce armor by 10%

    private float ignitedTimer = 2.5f;
    private float ignitedDamageCooldown = .5f;
    private float ignitedDamageTimer;

    public int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue() + vitality.GetValue();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;
        if (ignitedTimer < 0)
            isIgnited = false;

        if (ignitedDamageTimer < 0 && isIgnited)
        {
            TakeDamage(1);
            //Debug.Log("Take burn damage");
            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }

    public virtual void CheckLV()
    {
        if (experience_point.GetValue() == 5) // lv
        {
            lv.AddValue(1);
            attri_points.AddValue(1);
            experience_point.SetValue(0);
        }
        Debug.Log("LV Up");
    }

    public virtual void DoDamage(CharacterStats _targetStat)
    {
        if (CanAvoidAttack(_targetStat))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();
        totalDamage = CheckTargetArmor(_targetStat, totalDamage);
        _targetStat.TakeDamage(totalDamage); // physics damage
        //DoMagicDamage(_targetStat); // magic damage
    }


    public virtual void DoDamage(CharacterStats _targetStat, Vector2 _pos)
    {
        int totalDamage = damage.GetValue() + strength.GetValue();
        totalDamage = CheckTargetArmor(_targetStat, totalDamage);
        _targetStat.TakeDamage(totalDamage, _pos);
        //DoMagicDamage(_targetStat, _pos); // magic damage
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        //Debug.Log(_damage);

        if (currentHealth <= 0)
            Die();
    }

    public virtual void TakeDamage(int _damage, Vector2 _pos)
    {
        currentHealth -= _damage;
        if (isIgnited)
            currentHealth -= 1;
        //Debug.Log(_damage);

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died!");
        //throw new NotImplementedException();
    }
    private bool CanAvoidAttack(CharacterStats _targetStat)
    {
        int totalEvasion = _targetStat.evasion.GetValue() + _targetStat.aglity.GetValue();
        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Attack Avoided");
            return true;
        }
        return false;
    }

    private int CheckTargetArmor(CharacterStats _targetStat, int totalDamage)
    {
        //Debug.Log(_targetStat.armor.GetValue());
        int currentArmor = _targetStat.isChilled ? Mathf.RoundToInt(_targetStat.armor.GetValue() * .8f) : _targetStat.armor.GetValue();
        totalDamage -= currentArmor;
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    public virtual void DoMagicDamage(CharacterStats _targetStat) // enemy
    {
        int _fireDamage = element.GetValue() == 1 ? fireDamage.GetValue() : 0;
        int _iceDamage = element.GetValue() == 2 ? iceDamage.GetValue() : 0;
        int totalMagicDamage = _fireDamage + _iceDamage + intelligence.GetValue();
        totalMagicDamage -= magicResistance.GetValue() + intelligence.GetValue() * 3;
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);

        _targetStat.TakeDamage(totalMagicDamage);
    }

    public virtual void DoMagicDamage(CharacterStats _targetStat, Vector2 playerPos) // player
    {
        int _fireDamage = element.GetValue() == 1 ? fireDamage.GetValue() : 0;
        int _iceDamage = element.GetValue() == 2 ? iceDamage.GetValue() : 0;
        int totalMagicDamage = _fireDamage + _iceDamage + intelligence.GetValue();
        totalMagicDamage -= magicResistance.GetValue() + intelligence.GetValue() * 3;
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);

        _targetStat.TakeDamage(totalMagicDamage, playerPos);
    }
    public void ApplyAilments(bool _ignite, bool _chill)
    {
        if (_ignite || _chill)
            return;

        isIgnited = _ignite;
        isChilled = _chill;
    }
}
