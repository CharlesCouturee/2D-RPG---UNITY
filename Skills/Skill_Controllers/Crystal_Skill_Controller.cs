using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    Animator anim => GetComponent<Animator>();
    CircleCollider2D cd => GetComponent<CircleCollider2D>();
    Player player;

    float crystalExistTimer;
    bool canExplode;
    bool canMoveToEnemy;
    float moveSpeed;

    bool canGrow;
    float growSpeed = 5f;

    Transform closestEnemy;
    [SerializeField] LayerMask whatIsEnemy;

    public void SetUpCrystal(float _crystalDuartion, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, Transform _closestEnemy, Player _player)
    {
        player = _player;
        crystalExistTimer = _crystalDuartion;
        canExplode = _canExplode;
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackHoleRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
            closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
    }

    void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMoveToEnemy)
        {
            if (closestEnemy == null)
                return;

            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestEnemy.position) < 1f)
            {
                canMoveToEnemy = false;
                FinishCrystal();
            }

        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3f, 3f), growSpeed * Time.deltaTime);
    }

    void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.GetComponent<Entity>().SetUpKnockbackDirection(transform);
                player.stats.DoMagicalDamage(enemy.GetComponent<CharacterStats>());

                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                if (equipedAmulet != null)
                    equipedAmulet.Effect(enemy.transform);
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            anim.SetTrigger("Explode");
            canGrow = true;
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}
