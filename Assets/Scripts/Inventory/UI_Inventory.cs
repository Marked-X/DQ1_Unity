using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory : UI
{
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Transform itemContainer;

    public event EventHandler OnInventoryClosed;
    public bool IsInventoryEmpty
    {
        get
        {
            if(inventory.GetItemList().Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private enum State
    {
        Default,
        Inventory,
        ItemSelected,
        Confirmation
    }

    private State currentState;
    private GameObject itemSelectedMenu;
    private GameObject confirmationMenu;
    private Transform itemTemplate;
    private GameObject currentSelectedItem;
    private GameObject firstInventoryItem;


    private void OnEnable()
    {
        inventory.OnItemListChanged += OnItemListChange;
    }

    private void Awake()
    {
        itemTemplate = itemContainer.transform.Find("Item Template");
        itemSelectedMenu = gameObject.transform.Find("Item Selected Menu").gameObject;
        confirmationMenu = itemSelectedMenu.transform.Find("Confirmation Menu").gameObject;
        RefreshInventoryItems();
    }

    private void OnDisable()
    {
        inventory.OnItemListChanged -= OnItemListChange;
    }

    private void OnItemListChange(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    public void CloseMenu()
    {
        switch (currentState)
        {
            case State.Inventory:
                OnInventoryClosed?.Invoke(this, EventArgs.Empty);
                break;
            case State.ItemSelected:
                UnFade();
                EventSystem.current.SetSelectedGameObject(currentSelectedItem);
                itemSelectedMenu.SetActive(false);
                currentState = State.Inventory;
                break;
            case State.Confirmation:
                EventSystem.current.SetSelectedGameObject(itemSelectedMenu.transform.Find("Use Button").gameObject);
                confirmationMenu.SetActive(false);
                DialogManager.Instance.EndDialog();
                currentState = State.ItemSelected;
                break;
            case State.Default:
            default:
                Debug.Log("Inventory state broke");
                break;
        }
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        currentState = State.Inventory;
        EventSystem.current.SetSelectedGameObject(firstInventoryItem);
    }

    private void RefreshInventoryItems()
    {
        ClearItems(itemTemplate, itemContainer);

        GameObject previousItem = null;

        foreach(Item item in inventory.GetItemList())
        {
            GameObject currentItem = Instantiate(itemTemplate, itemContainer).gameObject;

            //item activation MOVE
            currentItem.GetComponent<ItemObject>().SetItem(item);
            currentItem.GetComponent<Button>().onClick.AddListener(ItemPressed);

            currentItem.SetActive(true);

            if (previousItem == null)
            {
                firstInventoryItem = currentItem;
                previousItem = currentItem;
                continue;
            }

            SetNavigationUp(previousItem, currentItem);
            SetNavigationDown(previousItem, currentItem);

            previousItem = currentItem;
        }
    }

    private void ItemPressed()
    {
        Fade();
        itemSelectedMenu.SetActive(true);
        currentState = State.ItemSelected;
        currentSelectedItem = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(itemSelectedMenu.transform.Find("Use Button").gameObject);
    }

    private void CloseInventory()
    {
        confirmationMenu.SetActive(false);
        itemSelectedMenu.SetActive(false);
        currentState = State.Inventory;

        OnInventoryClosed?.Invoke(this, EventArgs.Empty);
    }

    #region Item Selected Buttons
    public void Use()
    {
        itemSelectedMenu.SetActive(false);
        currentSelectedItem.GetComponent<ItemObject>().Item.Activate();

        DialogManager.Instance.StartDialog(new List<string>() { "Hero uses an item" });
        DialogManager.Instance.OnDialogEnded += OnUseDialogEnded;
    }

    private void OnUseDialogEnded(object sender, EventArgs e)
    {
        DialogManager.Instance.OnDialogEnded -= OnUseDialogEnded;
        UnFade();
        CloseInventory();
    }

    public void Discard()
    {
        confirmationMenu.SetActive(true);
        currentState = State.Confirmation;

        DialogManager.Instance.ShowLine("Wanna dis dis?");
        EventSystem.current.SetSelectedGameObject(null);
        DialogManager.Instance.OnLineEnded += OnDiscardLineEnded;
    }

    private void OnDiscardLineEnded(object sender, EventArgs e)
    {
        DialogManager.Instance.OnLineEnded -= OnDiscardLineEnded;
        EventSystem.current.SetSelectedGameObject(confirmationMenu.transform.Find("Yes Button").gameObject);
    }
    #endregion

    #region Confirmation Menu Buttons
    public void YesButton()
    {
        DialogManager.Instance.EndDialog();
        inventory.RemoveItem(currentSelectedItem.GetComponent<ItemObject>().Item);

        confirmationMenu.SetActive(false);
        itemSelectedMenu.SetActive(false);
        UnFade();

        if (inventory.GetItemList().Count == 0)
        {
            CloseInventory();
            return;
        }

        currentState = State.Inventory;
        EventSystem.current.SetSelectedGameObject(firstInventoryItem);
    }

    public void NoButton()
    {
        DialogManager.Instance.EndDialog();
        confirmationMenu.SetActive(false);
        currentState = State.ItemSelected;
        EventSystem.current.SetSelectedGameObject(itemSelectedMenu.transform.Find("Use Button").gameObject);
    }
    #endregion
}
