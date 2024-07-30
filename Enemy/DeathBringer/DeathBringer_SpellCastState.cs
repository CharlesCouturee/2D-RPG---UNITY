using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringer_SpellCastState : EnemyState
{
    Enemy_DeathBringer enemy;
    int amountOfSpell;
    float spellTimer;

    public DeathBringer_SpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpell = enemy.amountOfSpells;
        spellTimer = 0.5f;

        AudioManager.instance.PlaySFX(12, null);
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
            enemy.CastSpell();

        if (amountOfSpell <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeCast = Time.time;
    }

    bool CanCast()
    {
        if (amountOfSpell > 0 && spellTimer < 0)
        {
            amountOfSpell--;
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }
}
