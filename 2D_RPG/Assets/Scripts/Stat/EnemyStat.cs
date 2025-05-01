using UnityEngine;

public class EnemyStat : CharacterStats
{
    private Enemy enemy;
    //private PlayerStat player;
    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
        //player = GameObject.Find("Player").GetComponent<PlayerStat>();
        //if (player == null)
        //    Debug.Log("111");
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
        //player.LVup();
        enemy.Die();
    }
}
