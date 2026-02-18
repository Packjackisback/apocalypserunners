using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemData itemData;
    public int quantity = 1;
    public Inventory inventory;
    public InventoryUI inventoryUI;


    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (inventory != null)
            {
                bool added = inventory.AddItem(itemData, quantity);
                inventoryUI.Refresh();

                if (added)
                {
                    Debug.Log($"Picked up {quantity} {itemData.displayName}");
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventory is full!");
                }
            }
        }
    }
}