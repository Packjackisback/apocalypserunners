using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int size = 16;
    public List<ItemInstance> items = new List<ItemInstance>();
    private int selected = 0;

    void Awake()
    {
        items = new List<ItemInstance>(size);

        for (int i = 0; i < size; i++)
        {
            items.Add(null);
        }
    }

    public bool AddItem(ItemData data, int quantity)
    {
        // Try stacking first
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null &&
                items[i].data == data &&
                items[i].quantity < data.maxStack)
            {
                int spaceLeft = data.maxStack - items[i].quantity;
                int amountToAdd = Mathf.Min(spaceLeft, quantity);

                items[i].quantity += amountToAdd;
                quantity -= amountToAdd;

                if (quantity <= 0)
                    return true;
            }
        }

        // Add to empty slots
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                int amountToAdd = Mathf.Min(data.maxStack, quantity);
                items[i] = new ItemInstance(data, amountToAdd);

                quantity -= amountToAdd;

                if (quantity <= 0)
                    return true;
            }
        }

        return false;
    }

    public ItemInstance GetItem(int index)
    {
        if (index < 0 || index >= items.Count)
            return null;

        return items[index];
    }

    public void SetSelected(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            selected = index;
        }
    }

    public int GetSelected()
    {
        return selected;
    }

    public ItemInstance GetSelectedItem()
    {
        return items[selected];
    }
}