using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] CharacterStats targetStats;
    [SerializeField] float speed;
    int damage;

    Animator anim;
    bool triggered;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetUp(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    void Update()
    {
        if (!targetStats)
            return;

        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3f, 3f);
            anim.transform.localPosition = new Vector3(0, 0.5f);

            Invoke("DamageAndSelfDestroy", 0.2f);
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }

    void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, 0.4f);
    }
}
