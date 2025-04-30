using UnityEngine;

public class PlayerStat : CharacterStats
{
    private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        player.Damage();
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }
}
