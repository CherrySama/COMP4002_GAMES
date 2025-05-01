using UnityEngine;

public class PlayerUIState : PlayerState
{
    public PlayerUIState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.uiController.ShowUI();
        AudioManager.Instance.PlaySFX("UI");
    }

    public override void Exit()
    {
        base.Exit();

        player.uiController.HideUI();
        AudioManager.Instance.StopSFX("UI");
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.U))
            stateMachine.ChangeState(player.idleState);
    }
}
