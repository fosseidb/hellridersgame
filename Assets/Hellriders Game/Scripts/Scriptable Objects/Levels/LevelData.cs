using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int levelID;
    public string levelName;
    public string levelDescription;
    public Sprite levelWallpaper;
    public bool unlocked;
}
