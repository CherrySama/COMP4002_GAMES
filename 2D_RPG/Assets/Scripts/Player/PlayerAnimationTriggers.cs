using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponent<Player>();

    public void AttackFinished()
    {
        player.AnimationTrigger();
    }
}
