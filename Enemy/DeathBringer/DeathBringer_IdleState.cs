using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringer_IdleState : EnemyState
{
    Enemy_DeathBringer enemy;
    Transform player;

    public DeathBringer_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(player.position, enemy.transform.position) < 15f)
            enemy.bossFightBegins = true;


        if (stateTimer < 0 && enemy.bossFightBegins)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
