using Assets.Scripts.Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Default Item")]
public class Item : ScriptableObject, IItemActivation
{
    public string Name = "Item";
    public int Price = 0;

    public virtual void Activate()
    {
        Debug.Log("Emty Item Activation");
    }
}
