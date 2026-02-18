using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItemData : ItemData
{
    public int damage;
    public float fireRate;
    public float spread; //scale 0 - 1
    public int shots;
}
