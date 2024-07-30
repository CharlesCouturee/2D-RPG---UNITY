using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive_Controller : MonoBehaviour
{
    Animator anim;
    CharacterStats myStats;
    float growSpeed = 15f;
    float maxSize = 6f;
    float explosionRadius;

    bool canGrow = true;

    void Update()
    {
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (maxSize - transform.localScale.x < 0.5f)
        {
            canGrow = false;
            anim.SetTrigger("Explode");
        }
    }

    public void SetupExplosive(CharacterStats _myStats, float _growSpeed, float _maxSize, float _radius)
    {
        anim = GetComponent<Animator>();

        myStats = _myStats;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        explosionRadius = _radius;
    }

    void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in colliders)
        {
            CharacterStats enemy = hit.GetComponent<CharacterStats>();
            if (enemy)
            {
                enemy.GetComponent<Entity>().SetUpKnockbackDirection(transform);
                myStats.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }

    void SelfDestroy() => Destroy(gameObject);
}
