using TMPro;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField]
    private GameObject text;
    public Item Item { get; private set; }

    public void SetItem(Item item)
    {
        this.Item = item;
        text.GetComponent<TextMeshProUGUI>().text = item.Name;
    }
}
