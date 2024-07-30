using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/Item Effect/Freeze Enemies Effect")]
public class FreezeEnemies_Effect : Item_Effect
{
    [SerializeField] float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (stats.currentHealth > (stats.GetMaxHealthValue() * 0.1f))
            return;


        if (!Inventory.instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2f);
        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
                enemy.FreezeTimeFor(duration);
        }
    }
}
