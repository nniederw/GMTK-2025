using UnityEngine;
using UnityEngine.Windows;
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public uint DamageIncrease = 0;
    public uint DamageAbsorbtion = 0;
    public Sprite Sprite;
    public ItemClass Class;
    public string GetText()
    {
        var result = "";
        if (DamageIncrease != 0)
        {
            result += $"Damage +{DamageIncrease}\n";
        }
        if (DamageAbsorbtion != 0)
        {
            result += $"Armor +{DamageAbsorbtion}\n";
        }
        return result != "" ? result.Substring(0, result.Length - 1) : "";
    }
    //public uint DashBonus = 0;
}
public enum ItemClass { Sword, Shield, Armor, Amulet }