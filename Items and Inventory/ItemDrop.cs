using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] int possibleItemDrop;
    [SerializeField] ItemData[] possibleDrops;
    List<ItemData> dropList = new List<ItemData>();

    [SerializeField] GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        if (possibleDrops.Length == 0)
        {
            Debug.Log("Item Pool is empty, can't drop items");
            return;
        }

        foreach (ItemData item in possibleDrops)
        {
            if (item != null && Random.Range(0, 100) < item.dropChance)
                dropList.Add(item);
        }

        for (int i = 0; i < possibleItemDrop; i++)
        {
            if (dropList.Count > 0)
            {
                ItemData itemToDrop = dropList[Random.Range(0, dropList.Count)];

                DropItem(itemToDrop);
                dropList.Remove(itemToDrop);
            }
        }
    }
    
    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
