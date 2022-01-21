using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : Interactable
{
    [SerializeField]
    private List<string> DialogArray;
    [SerializeField]
    private Item item = null;

    public override void Activate()
    {
        if(item != null)
        {
            DialogManager.Instance.StartDialog(DialogArray);
            GameManager.Instance.player.GetComponent<Player>().AddItem(item);
        }
        else
        {
            DialogManager.Instance.StartDialog(DialogArray);
        }
    }
}