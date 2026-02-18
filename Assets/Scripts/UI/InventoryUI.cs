using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public InventorySlotUI slotPrefab;
    public int slotCount = 6;

    private InventorySlotUI[] slots;
    private int selectedSlot = 0;

    void Start()
    {
        slots = new InventorySlotUI[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            InventorySlotUI slot = Instantiate(slotPrefab, transform);
            slot.Setup(this, i);
            slots[i] = slot;
        }

        Refresh();
        SelectSlot(0);
    }

    public void Refresh()
    {
        for (int i = 0; i < slotCount; i++)
        {
            slots[i].SetItem(inventory.items[i]);
        }
    }

    public void SelectSlot(int index)
    {
        selectedSlot = index;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSelected(i == selectedSlot);
            if (i == selectedSlot) Debug.Log("Selected slot " + i);
        }
    }

    public ItemInstance GetSelectedItem()
    {
        return inventory.items[selectedSlot];
    }
}
