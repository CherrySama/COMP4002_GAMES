using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponent<Player>();

    public void AttackFinished()
    {
        player.AnimationTrigger();
    }

    public void LadderClimbFinished()
    {
        player.AnimationTrigger();
    }
}
