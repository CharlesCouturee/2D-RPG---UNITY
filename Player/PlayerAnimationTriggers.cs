using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2, null);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                if (enemy.GetComponent<EnemyStats>() != null)
                    player.stats.DoDamage(enemy.stats);

                ItemData_Equipment weaponDate = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                if (weaponDate != null)
                    weaponDate.Effect(enemy.transform);
            }

        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
