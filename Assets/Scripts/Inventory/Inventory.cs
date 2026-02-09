using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int size = 16;
    public List<ItemInstance> items = new List<ItemInstance>();

    private void Awake()
    {
        for (int i = 0; i < size; i++)
            items.Add(null);
    }

    public bool AddItem(ItemData data, int quantity)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null &&
                items[i].data == data &&
                items[i].quantity < data.maxStack)
            {
                items[i].quantity += quantity;
                return true;
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = new ItemInstance(data, quantity);
                return true;
            }
        }

        return false;
    }
}
