using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private EnemyList enemyList;
    [SerializeField]
    private BackgroundList backgroundList;
    [SerializeField]
    private GameObject battleMenu;
    [SerializeField]
    private Image backgroundSprite;
    [SerializeField]
    private Image enemySprite;
    [SerializeField]
    private GameObject actionMenu;
    [SerializeField]
    private GameObject attackButton;


    private Player player;
    private Dictionary<EnemyList.EnemyType, Enemy> enemyDictionary;
    private Dictionary<BackgroundList.BackgroundType, Sprite> backgroundDictionary;
    private bool isPlayerTurn;
    private Enemy currentEnemy;


    private void Start()
    {
        enemyDictionary = enemyList.GetEnemyDictionary();
        backgroundDictionary = backgroundList.GetBackgroundDictionary();
        player = (Player)GameManager.Instance.player.GetComponent("Player");
    }

    private void FixedUpdate()
    {
    }

    public void StartBattle(BackgroundList.BackgroundType background, EnemyList.EnemyType enemy)
    {
        isPlayerTurn = true;

        battleMenu.SetActive(true);
        GameManager.Instance.ChangeState(GameManager.States.Battle);

        currentEnemy = enemyDictionary[enemy];
        currentEnemy.ReadyUp();
        backgroundSprite.sprite = backgroundDictionary[background];
        enemySprite.sprite = enemyDictionary[enemy].Sprite;

        NextTurn();
    }

    private void NextTurn()
    {
        if (isPlayerTurn)
        {
            PlayerTakeTurn();
        }
        else
        {
            EnemyTakeTurn();
        }
        isPlayerTurn = !isPlayerTurn;
    }

    private void PlayerTakeTurn()
    {
        actionMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(attackButton);
    }

    private void EnemyTakeTurn()
    {
        actionMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

    }



    public void AttackButton()
    {
        int attackDamage = player.GetStatList()[Stats.StatType.Attack];
        currentEnemy.CurrentHP = currentEnemy.CurrentHP - attackDamage;
        if(currentEnemy.CurrentHP <= 0)
        {
            Victory();
        } else
        {
            NextTurn();
        }
    }



    private void Victory()
    {

    }

    private void Lose()
    {

    }
}
