using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Status : UI
{
    public event EventHandler OnStatusClosed;

    private GameObject equipmentMenu;
    private GameObject statMenu;
    private GameObject equipmentButton;
    Dictionary<Stats.StatType, TextMeshProUGUI> statTextObjects;
    private State currentState;
    private Player player;

    private enum State
    {
        Default,
        Equipment,
        Stats
    }

    private void Awake()
    {
        equipmentMenu = transform.Find("Equip Menu").gameObject;
        equipmentButton = transform.Find("Equip Button").gameObject;
        statMenu = transform.Find("Stat Menu").gameObject;

        player = GameManager.Instance.player.GetComponent<Player>();

        statTextObjects = new Dictionary<Stats.StatType, TextMeshProUGUI>();
        statTextObjects.Add(Stats.StatType.Level, GameObject.Find("UI/Status Menu/Stat Menu/Character Info/Stats/Level").GetComponent<TextMeshProUGUI>());
        statTextObjects.Add(Stats.StatType.CurrentHP, GameObject.Find("UI/Status Menu/Stat Menu/Character Info/Stats/HP").GetComponent<TextMeshProUGUI>());
        statTextObjects.Add(Stats.StatType.CurrentMP, GameObject.Find("UI/Status Menu/Stat Menu/Character Info/Stats/MP").GetComponent<TextMeshProUGUI>());

        statTextObjects.Add(Stats.StatType.Strength, GameObject.Find("UI/Status Menu/Stat Menu/Stat Info/Stats/Strength").GetComponent<TextMeshProUGUI>());
        statTextObjects.Add(Stats.StatType.Agility, GameObject.Find("UI/Status Menu/Stat Menu/Stat Info/Stats/Agility").GetComponent<TextMeshProUGUI>());
        statTextObjects.Add(Stats.StatType.MaxHP, GameObject.Find("UI/Status Menu/Stat Menu/Stat Info/Stats/Max HP").GetComponent<TextMeshProUGUI>());
        statTextObjects.Add(Stats.StatType.MaxMP, GameObject.Find("UI/Status Menu/Stat Menu/Stat Info/Stats/Max MP").GetComponent<TextMeshProUGUI>());
        statTextObjects.Add(Stats.StatType.Attack, GameObject.Find("UI/Status Menu/Stat Menu/Stat Info/Stats/Attack").GetComponent<TextMeshProUGUI>());
        statTextObjects.Add(Stats.StatType.Defense, GameObject.Find("UI/Status Menu/Stat Menu/Stat Info/Stats/Defense").GetComponent<TextMeshProUGUI>());
        statTextObjects.Add(Stats.StatType.EXP, GameObject.Find("UI/Status Menu/Stat Menu/Stat Info/Stats/EXP").GetComponent<TextMeshProUGUI>());
    }

    private void OnEnable()
    {
        equipmentMenu.GetComponent<UI_Equipment>().OnEquipMenuClose += OnEquipmentClose;
        player.OnStatChange += OnStatChange;
    }

    private void OnDisable()
    {
        equipmentMenu.GetComponent<UI_Equipment>().OnEquipMenuClose -= OnEquipmentClose;
        player.OnStatChange -= OnStatChange;
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        currentState = State.Default;
        EventSystem.current.SetSelectedGameObject(equipmentButton);
    }

    public void CloseMenu()
    {
        switch (currentState)
        {
            case State.Default:
                StatusClose();
                break;
            case State.Equipment:
                equipmentMenu.GetComponent<UI_Equipment>().CloseMenu();
                break;
            case State.Stats:
                StatClose();
                break;
            default:
                Debug.Log("UI_Status state broke");
                break;
        }
    }

    public void EquipmentOpen()
    {
        equipmentMenu.GetComponent<UI_Equipment>().OpenMenu();
        Fade();
        currentState = State.Equipment;
    }

    private void OnEquipmentClose(object sender, EventArgs e)
    {
        UnFade();
        equipmentMenu.SetActive(false);
        currentState = State.Default;
        EventSystem.current.SetSelectedGameObject(equipmentButton);
    }

    public void StatOpen()
    {
        Fade();
        statMenu.SetActive(true);
        currentState = State.Stats;
        EventSystem.current.SetSelectedGameObject(statMenu);
    }

    private void StatClose()
    {
        UnFade();
        statMenu.SetActive(false);
        currentState = State.Default;
        EventSystem.current.SetSelectedGameObject(equipmentButton);
    }

    public void OnStatusButton()
    {
        //magic menu
        OnStatChange(this, EventArgs.Empty);
    }

    private void StatusClose()
    {
        UnFade();
        currentState = State.Default;
        OnStatusClosed?.Invoke(this, EventArgs.Empty);
    }

    private void OnStatChange(object sender, EventArgs e)
    {
        Dictionary<Stats.StatType, int> temp = player.GetStatList();
        foreach(KeyValuePair<Stats.StatType, int> pair in temp)
        {
            statTextObjects[pair.Key].text = pair.Value.ToString(); 
        }
    }
}
