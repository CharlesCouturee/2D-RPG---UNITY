using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    Enemy enemy => GetComponentInParent<Enemy>();

    void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
        {
            Player player = hit.GetComponent<Player>();
            if (player)
                enemy.stats.DoDamage(player.stats);
        }
    }

    void SpecialAttackTrigger()
    {
        enemy.AnimationSpecialAttackTrigger();
    }

    void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
