using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice And Fire Effect", menuName = "Data/Item Effect/Ice And Fire")]
public class IceAndFire_Effect : Item_Effect
{
    [SerializeField] GameObject IceAndFirePrefab;
    [SerializeField] float xVelocity;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;
        bool thirAttack = player.primaryAttackState.comboCounter == 2;

        if (thirAttack)
        {
            GameObject newIceAndFire = Instantiate(IceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0f);

            Destroy(newIceAndFire, 10f);
        }

    }
}
