using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Animator anim;

    public string id;
    public bool activationStatus;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint ID")]
    void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            ActivateCheckpoint();
    }

    public void ActivateCheckpoint()
    {
        if (!activationStatus)
            AudioManager.instance.PlaySFX(5, transform);

        activationStatus = true;
        anim.SetBool("Active", true);
    }
}
