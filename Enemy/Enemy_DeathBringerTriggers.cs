using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringerTriggers : EnemyAnimationTriggers
{
    Enemy_DeathBringer enemyDeathBringer => GetComponentInParent<Enemy_DeathBringer>();

    void Relocate() => enemyDeathBringer.FindPosition();

    void MakeInvisible() => enemyDeathBringer.fx.MakeTransparent(true);
    void MakeVisible() => enemyDeathBringer.fx.MakeTransparent(false);
}
