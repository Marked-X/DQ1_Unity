using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField]
    private List<string> DialogArray;

    public override void Activate()
    {
        DialogManager.Instance.StartDialog(DialogArray);
    }
}
