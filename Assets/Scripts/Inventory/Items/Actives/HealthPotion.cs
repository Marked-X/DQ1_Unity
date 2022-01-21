using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Active/HealthPotion")]
public class HealthPotion : Item
{
    public override void Activate()
    {
        Debug.Log("Health potion used");
    }
}
