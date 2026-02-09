[System.Serializable]
public class ItemInstance
{
    public ItemData data;
    public int quantity;

    public ItemInstance(ItemData data, int quantity)
    {
        this.data = data;
        this.quantity = quantity;
    }
}
