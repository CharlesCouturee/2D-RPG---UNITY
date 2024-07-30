using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Transform craftSlotParent;
    [SerializeField] GameObject craftSlotPrefab;

    [SerializeField] List<ItemData_Equipment> craftEquipment = new List<ItemData_Equipment>();

    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetUpCraftList();
        SetUpDefaultCraftWindow();
    }
    public void SetUpCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetUpCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetUpCraftList();
    }

    public void SetUpDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<UI>().craftWindow.SetUpCraftWindow(craftEquipment[0]);
    }
}
