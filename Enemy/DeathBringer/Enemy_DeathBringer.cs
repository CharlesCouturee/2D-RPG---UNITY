using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    #region States
    public DeathBringer_BattleState battleState {  get; private set; }
    public DeathBringer_AttackState attackState { get; private set; }
    public DeathBringer_IdleState idleState { get; private set; }
    public DeathBringer_DeadState deadState { get; private set; }
    public DeathBringer_SpellCastState spellCastState { get; private set; }
    public DeathBringer_TeleportState teleportState { get; private set; }
    #endregion

    public bool bossFightBegins;

    [Header("Spell Cast Details")]
    [SerializeField] GameObject spellPrefab;
    public int amountOfSpells;
    public float spellCooldown;
    public float lastTimeCast;
    [SerializeField] float spellStateCooldown;

    [Header("Teleport Details")]
    [SerializeField] BoxCollider2D arena;
    [SerializeField] Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25f;

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new DeathBringer_IdleState(this, stateMachine, "Idle", this);

        battleState = new DeathBringer_BattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringer_AttackState(this, stateMachine, "Attack", this);

        deadState = new DeathBringer_DeadState(this, stateMachine, "Idle", this);
        spellCastState = new DeathBringer_SpellCastState(this, stateMachine, "SpellCast", this);
        teleportState = new DeathBringer_TeleportState(this, stateMachine, "Teleport", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;

        float xOffset = 0f;
        if (player.rb.velocity.x != 0)
            xOffset = player.facingDir * 1.5f;

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + 1.5f);

        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        newSpell.GetComponent<DeathBringerSpell_Controller>().SetupSpell(stats);
    }

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3f, arena.bounds.max.x - 3f);
        float y = Random.Range(arena.bounds.min.y + 3f, arena.bounds.max.y - 3f);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            Debug.Log("Loosikng for new position");
            FindPosition();
        }
    }

    RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanDoSpellCast()
    {
        if (Time.time >= lastTimeCast + spellStateCooldown)
        {
            return true;
        }

        return false;
    }
}
