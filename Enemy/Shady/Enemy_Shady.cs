using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Enemy_Shady : Enemy
{
    [Header("Shady Specific")]
    public float battleStateMoveSpeed;

    [SerializeField] GameObject explosivePrefab;
    [SerializeField] float growSpeed;
    [SerializeField] float maxSize;

    #region States
    public ShadyIdleState idleState {  get; set; }
    public ShadyMoveState moveState { get; set; }
    public ShadyDeadState deadState { get; set; }
    public ShadyStunnedState stunnedState { get; set; }
    public ShadyBattleState battleState { get; set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);
        deadState = new ShadyDeadState(this, stateMachine, "Dead", this);
        stunnedState = new ShadyStunnedState(this, stateMachine, "Stunned", this);
        battleState = new ShadyBattleState(this, stateMachine, "MoveFast", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        AudioManager.instance.PlaySFX(29, transform);
        GameObject newExplosive = Instantiate(explosivePrefab, attackCheck.position, Quaternion.identity);
        newExplosive.GetComponent<Explosive_Controller>().SetupExplosive(stats, growSpeed, maxSize, attackCheckRadius);

        cd.enabled = false;
        rb.gravityScale = 0;
    }

    public void SelfDestroy() => Destroy(gameObject);
}
