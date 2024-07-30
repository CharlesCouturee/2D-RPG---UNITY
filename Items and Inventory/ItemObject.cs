using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ItemData itemData;

    private void SetupVisual()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object " + itemData.name;
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisual();
    }

    public void PickupItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0f, 7f);
            PlayerManager.instance.player.fx.CreatePopUpText("Inventory is Full");
            return;
        }

        AudioManager.instance.PlaySFX(18, transform);

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
