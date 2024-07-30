using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    Enemy_Slime enemy;

    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.stunDuration;

        enemy.fx.InvokeRepeating("RedColorBlink", 0f, 0.1f);
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0.1f && enemy.IsGroundDetected())
        {
            enemy.fx.Invoke("CancelColorChange", 0f);
            enemy.anim.SetTrigger("StunFold");
            enemy.stats.MakeInvincible(true);
        }

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.MakeInvincible(false);
    }
}
