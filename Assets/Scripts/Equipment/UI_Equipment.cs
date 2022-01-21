using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Equipment : UI
{
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Transform itemContainer;
    [SerializeField]
    private Item emptyItem;

    public event EventHandler OnEquipMenuClose;

    private State currentState;
    private Transform itemTemplate;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemCategory;
    private TextMeshProUGUI statName;
    private TextMeshProUGUI statChange;
    private GameObject firstItem;
    private Dictionary<Stats.StatType, int> stats;

    private enum State
    {
        Default,
        Weapon,
        Armor,
        Shield,
        Accessory,
        LastState
    }

    private void Awake()
    {
        itemTemplate = itemContainer.transform.Find("Item Template");
        itemName = transform.Find("Info Menu").Find("Item Name").gameObject.GetComponent<TextMeshProUGUI>();
        statName = transform.Find("Info Menu").Find("Stat").gameObject.GetComponent<TextMeshProUGUI>();
        statChange = transform.Find("Info Menu").Find("Numbers").gameObject.GetComponent<TextMeshProUGUI>();
        itemCategory = transform.Find("Item Menu").Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        stats = GameManager.Instance.player.GetComponent<Player>().GetStatList();
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        currentState = State.Weapon;
        ItemMenuOpen();
    }

    public void CloseMenu()
    {
        currentState = State.Weapon;
        OnEquipMenuClose?.Invoke(this, EventArgs.Empty);
    }

    private void ItemMenuOpen()
    {
        switch (currentState)
        {
            case State.Weapon:
                itemCategory.text = "Weapon";
                statName.text = "Attack";
                FillItems(Equipment.Type.Weapon);
                break;
            case State.Shield:
                itemCategory.text = "Shield";
                statName.text = "Defense";
                FillItems(Equipment.Type.Shield);
                break;
            case State.Armor:
                itemCategory.text = "Armor";
                statName.text = "Defense";
                FillItems(Equipment.Type.Armor);
                break;
            case State.Accessory:
                itemCategory.text = "Accessory";
                FillItems(Equipment.Type.Accessory);
                break;
            case State.LastState:
                CloseMenu();
                break;
            default:
                Debug.Log("UI_equipment state error");
                break;
        }
    }

    private void FillItems(Equipment.Type type)
    {
        GameObject previousItem = null;
        firstItem = null;

        ClearItems(itemTemplate, itemContainer);

        foreach (Equipment item in inventory.GetEquipmentList())
        {
            if(item.equipmentType != type)
            {
                continue;
            }

            GameObject currentItem = Instantiate(itemTemplate, itemContainer).gameObject;

            currentItem.GetComponent<ItemObject>().SetItem(item);
            currentItem.GetComponent<Button>().onClick.AddListener(ItemPressed);

            currentItem.SetActive(true);

            if (previousItem == null)
            {
                firstItem = currentItem;
                previousItem = currentItem;
                continue;
            }

            SetNavigationUp(previousItem, currentItem);
            SetNavigationDown(previousItem, currentItem);

            previousItem = currentItem;
        }

        GameObject emptyItemObject = Instantiate(itemTemplate, itemContainer).gameObject;
        emptyItemObject.GetComponent<ItemObject>().SetItem(emptyItem);
        emptyItemObject.GetComponent<Button>().onClick.AddListener(ItemPressed);
        emptyItemObject.SetActive(true);
        if(currentState == State.Accessory)
        {
            emptyItemObject.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "end";
        }
        else
        {
            emptyItemObject.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Empty";
        }

        if (firstItem != null)
        {
            SetNavigationDown(previousItem, emptyItemObject);
            SetNavigationUp(previousItem, emptyItemObject);
            EventSystem.current.SetSelectedGameObject(firstItem);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(emptyItemObject);
        }
    }

    public void OnSelect(GameObject sender)
    {
        Equipment eq = (Equipment)sender.GetComponent<ItemObject>().Item;

        itemName.text = eq.Name;
        if(eq.Stat.Key == Stats.StatType.Attack)
        {
            statChange.text = stats[eq.Stat.Key] + " > " + (stats[Stats.StatType.Strength] + eq.Stat.Value);
        }
        else if (eq.Stat.Key == Stats.StatType.Defense)
        {
            statChange.text = stats[eq.Stat.Key] + " > " + (stats[Stats.StatType.Agility] + eq.Stat.Value);
        }
    }

    private void ItemPressed()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        GameManager.Instance.player.GetComponent<Player>().EquipItem((Equipment)selected.GetComponent<ItemObject>().Item);
        currentState++;
        ItemMenuOpen();
    }
}
