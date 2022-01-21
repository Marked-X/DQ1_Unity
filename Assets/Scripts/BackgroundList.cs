using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BackgroundList", menuName = "Game/BackgroundList")]
public class BackgroundList : ScriptableObject
{
    public enum BackgroundType
    {
        Default,
        Plains,
        Forest,
        Mountains,
        Rocks,
        Poison,
        Cave_Yellow,
        Cave_Gray,
        Ruins_Inside,
        Ruins_Outside,
        Castle_Corridor,
        Castle_Underground,
        Castle_Final
    }

    [SerializeField]
    private List<Sprite> images;

    public Dictionary<BackgroundType, Sprite> GetBackgroundDictionary()
    {
        Dictionary<BackgroundType, Sprite> temp = new Dictionary<BackgroundType, Sprite>();
        int i = 0;
        foreach(Sprite image in images)
        {
            temp.Add((BackgroundType)i++, image);
        }
        return temp;
    }
}
