using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpell_Controller : MonoBehaviour
{
    [SerializeField] Transform check;
    [SerializeField] Vector2 boxSize;
    [SerializeField] LayerMask whatIsPlayer;

    CharacterStats myStats;

    public void SetupSpell(CharacterStats stats) => myStats = stats;

    void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);
        foreach (var hit in colliders)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.GetComponent<Entity>().SetUpKnockbackDirection(transform);
                myStats.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
