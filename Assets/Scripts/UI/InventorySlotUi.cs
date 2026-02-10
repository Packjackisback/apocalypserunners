using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public Image selectionHighlight;

    private int slotIndex;
    private InventoryUI inventoryUI;

    public void Setup(InventoryUI ui, int index)
    {
        inventoryUI = ui;
        slotIndex = index;
    }

    public void SetItem(ItemInstance item)
    {
        if (item == null)
        {
            icon.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = item.data.icon;
        }
    }

    public void SetSelected(bool selected)
    {
        selectionHighlight.enabled = selected;
    }

    public void OnClick()
    {
        inventoryUI.SelectSlot(slotIndex);
    }
}
