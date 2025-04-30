using UnityEngine;

public class EnemyStat : CharacterStats
{
    private Enemy enemy;
    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage, Vector2 _playerPos)
    {
        base.TakeDamage(_damage, _playerPos);
        HitDamageShown.Instance.ShowDamageValue(_damage);
        enemy.Damage(_playerPos);
    }

    protected override void Die()
    {
        base.Die();
        if (LVup.Instance != null)
            LVup.Instance.PlayFlash();
        enemy.Die();
    }
}
