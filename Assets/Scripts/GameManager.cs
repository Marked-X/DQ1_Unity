using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private EventSystem eventSystem;

    private bool _isPaused = false;
    private bool _isInDialog = false;
    private PlayerMovement playerMovement;

    public GameObject player { get; private set; }
    public bool IsPaused
    {
        get 
        {
            return _isPaused;
        }
        set
        {
            if (value)
            {
                _isPaused = true;
                Time.timeScale = 0f;
            }
            else
            {
                _isPaused = false;            
                Time.timeScale = 1f;
            }
        }
    }
    public bool IsInDialog { 
        get 
        {
            return _isInDialog;
        } 
        set 
        {
            if (value)
            {
                _isInDialog = true;
            }
            else
            {
                _isInDialog = false;
            }
        } 
    }

    public enum States 
    {   
        Default, 
        Freeroam, 
        Dialog, 
        Menu,
        Battle
    }
    public States CurrentState { get; private set; } = States.Freeroam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangeState(States state)
    {
        switch (state)
        {
            case States.Freeroam:
                IsPaused = false;
                eventSystem.gameObject.SetActive(false);
                inputReader.EnableFreeroamInput();
                break;
            case States.Menu:
                IsPaused = true;
                eventSystem.gameObject.SetActive(true);
                inputReader.EnableMenuInput();
                break;
            case States.Dialog:
                IsPaused = true;
                eventSystem.gameObject.SetActive(false);
                inputReader.EnableDialogInput();
                break;
            case States.Battle:
                IsPaused = true;
                eventSystem.gameObject.SetActive(true);
                inputReader.EnableMenuInput();
                break;
            default:
                IsPaused = true;
                eventSystem.gameObject.SetActive(false);
                inputReader.DisableAllInput();
                Debug.Log("Broken State");
                break;
        }

        CurrentState = state;
    }
}
