using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : UI
{
    [SerializeField]
    private GameObject menuCanvas;
    [SerializeField]
    private GameObject statusMenu;
    [SerializeField]
    private GameObject itemMenu;
    [SerializeField]
    private GameObject statusSelectedButton;
    [SerializeField]
    private InputReader inputReader;

    private GameObject previousButton;

    private IRayCheck rayCheck;

    private enum MenuState
    {
        Default,
        Main,
        Status,
        Spell,
        Item,
        Equip
    }

    private MenuState currentState = MenuState.Main;

    private void OnEnable()
    {
        inputReader.openMenuEvent += OnOpenMenu;
        inputReader.closeMenuEvent += OnCloseMenu;
        itemMenu.GetComponent<UI_Inventory>().OnInventoryClosed += OnInventoryClosed;
        statusMenu.GetComponent<UI_Status>().OnStatusClosed += OnStatusClosed;
    }

    private void Awake()
    {
        rayCheck = GetComponent<IRayCheck>();
    }

    private void OnDisable()
    {
        inputReader.openMenuEvent -= OnOpenMenu;
        inputReader.closeMenuEvent -= OnCloseMenu;
        itemMenu.GetComponent<UI_Inventory>().OnInventoryClosed -= OnInventoryClosed;
        statusMenu.GetComponent<UI_Status>().OnStatusClosed -= OnStatusClosed;
    }

    public void OnOpenMenu()
    {
        menuCanvas.SetActive(true);
        currentState = MenuState.Main;
        GameManager.Instance.ChangeState(GameManager.States.Menu);
        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }

    public void OnCloseMenu()
    {
        switch (currentState)
        {
            case MenuState.Main:
                menuCanvas.SetActive(false);
                GameManager.Instance.ChangeState(GameManager.States.Freeroam);
                break;
            case MenuState.Status:
                statusMenu.GetComponent<UI_Status>().CloseMenu();
                break;
            case MenuState.Spell:
                //spell menu disable
                currentState = MenuState.Main;
                break;
            case MenuState.Item:
                itemMenu.GetComponent<UI_Inventory>().CloseMenu();
                break;
            default:
                Debug.Log("Unexpected menu state");
                break;
        }
    }

    private void OnInventoryClosed(object sender, EventArgs e)
    {
        EventSystem.current.SetSelectedGameObject(previousButton);
        itemMenu.SetActive(false);
        currentState = MenuState.Main;
        UnFade();
    }

    private void OnStatusClosed(object sender, EventArgs e)
    {
        EventSystem.current.SetSelectedGameObject(previousButton);
        statusMenu.SetActive(false);
        currentState = MenuState.Main;
        UnFade();
    }

    //----------Menu Buttons-----------

    public void OnTalk()
    {
        GameObject hit = rayCheck.CheckTag("NPC");
        if (hit != null)
        {
            hit.GetComponent<Interactable>().Activate();
        }
        else
        {
            DialogManager.Instance.FailedTaskDialog();
        }
    }

    public void OnSearch()
    {
        GameObject hit = rayCheck.CheckTag("Searchable");
        if (hit != null)
        {
            hit.GetComponent<Interactable>().Activate();
        }
        else
        {
            DialogManager.Instance.FailedTaskDialog();
        }
    }

    public void OnDoor()
    {
        GameObject hit = rayCheck.CheckTag("Door");
        if (hit != null)
        {
            hit.GetComponent<Interactable>().Activate();
        }
        else
        {
            DialogManager.Instance.FailedTaskDialog();
        }
    }

    public void OnStatus()
    {
        Fade();
        previousButton = EventSystem.current.currentSelectedGameObject;
        statusMenu.GetComponent<UI_Status>().OpenMenu();
        currentState = MenuState.Status;
    }

    public void OnSpell()
    {

    }

    public void OnItem()
    {
        Fade();
        if (!itemMenu.GetComponent<UI_Inventory>().IsInventoryEmpty)
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            itemMenu.GetComponent<UI_Inventory>().OpenMenu();
            currentState = MenuState.Item;
        }
        else
        {
            DialogManager.Instance.StartDialog(new List<string>() { "Inventory is empty" });
            UnFade();
        }
    }
}
