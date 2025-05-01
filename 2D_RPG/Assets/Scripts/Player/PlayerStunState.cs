using UnityEngine;

public class PlayerStunState : PlayerState
{
    public PlayerStunState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.stunDuration;

        AudioManager.Instance.PlaySFX("Hit");

        if (DamageFlashEffect.Instance != null)
            DamageFlashEffect.Instance.PlayFlash();
    }

    public override void Exit()
    {
        base.Exit();

        player.isHit = false;
        AudioManager.Instance.StopSFX("Hit");
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
