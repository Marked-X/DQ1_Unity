using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyList", menuName = "Game/EnemyList")]
public class EnemyList : ScriptableObject
{
    public enum EnemyType
    {
        Slime,
        Skeleton
    }

    [SerializeField]
    private List<Enemy> enemies;

    public Dictionary<EnemyType, Enemy> GetEnemyDictionary()
    {
        Dictionary<EnemyType, Enemy> temp = new Dictionary<EnemyType, Enemy>();
        int i = 0;
        foreach(Enemy enemy in enemies)
        {
            temp.Add((EnemyType)i++, enemy);
        }
        return temp;
    }
}
