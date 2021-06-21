using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Hellrider", menuName ="ScriptableObjects/HellriderData", order =1)]
public class HellriderData : ScriptableObject
{
    public string hellriderName;
    public float hellriderHealth;
    public float hellriderArmor;
    public float hellriderMaxSpeed;
    public float hellriderMaxAcceleration;
    public Sprite hellriderIcon;
    public bool unlocked;
}
