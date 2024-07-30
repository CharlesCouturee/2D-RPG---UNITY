using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    Player player;
    SpriteRenderer sr;
    Animator anim;
    [SerializeField] float colourLosingSpeed;

    float cloneTimer;
    float attackMultiplier;
    [SerializeField] Transform attackCheck;
    [SerializeField] float attackCheckRadius = 0.8f;
    Transform closestEnemy;
    int facingDir = 1;

    bool canDuplicateClone;
    float chanceToDuplicate;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        cloneTimer -= Time.time;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colourLosingSpeed));

            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public void SetUpClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _chanceToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));

        attackMultiplier = _attackMultiplier;
        player = _player;
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
        FaceClosestTarget();
        
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.GetComponent<Entity>().SetUpKnockbackDirection(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemData_Equipment weaponDate = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                    if (weaponDate != null)
                        weaponDate.Effect(enemy.transform);
                }

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(enemy.transform, new Vector3(0.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    void FaceClosestTarget()
    {
        if (closestEnemy)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
                facingDir = -1;
            }
        }
    }
}
