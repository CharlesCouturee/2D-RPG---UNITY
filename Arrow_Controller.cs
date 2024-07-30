using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    SpriteRenderer sr;

    [SerializeField] int damage;
    [SerializeField] string targetLayerName = "Player";

    [SerializeField] float xVelocity;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] bool canMove;
    [SerializeField] bool flipped;

    CharacterStats myStats;
    int facingDir = 1;

    private void Update()
    {
        if (canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);

        if (facingDir == 1 && rb.velocity.x < 0)
        {
            facingDir = -1;
            sr.flipX = true;
        }    
    }

    public void SetupArrow(float _speed, CharacterStats _myStats)
    {
        sr = GetComponent<SpriteRenderer>();
        xVelocity = _speed;
        myStats = _myStats;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            //collision.GetComponent<CharacterStats>()?.TakeDamage(damage);

            myStats.DoDamage(collision.GetComponent<CharacterStats>());
            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(5, 7));
    }

    public void FlipArrow()
    {
        if (flipped)
            return;

        xVelocity *= -1;
        flipped = true;
        transform.Rotate(0f, 180f, 0f);
        targetLayerName = "Enemy";
    }
}
