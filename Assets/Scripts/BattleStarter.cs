using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    [SerializeField]
    BattleManager battleManager;
    [SerializeField]
    BackgroundList.BackgroundType background;
    [SerializeField]
    EnemyList.EnemyType enemy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        battleManager.StartBattle(background, enemy);
        Destroy(gameObject);
    }
}
