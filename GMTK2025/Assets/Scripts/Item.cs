using UnityEngine;
using UnityEngine.Windows;
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public ItemRarity Rarity = ItemRarity.Common;
    public int DamageIncrease = 0;
    public int DamageAbsorbtion = 0;
    public Sprite Sprite;
    public ItemClass Class;
    public string GetText()
    {
        var result = "";
        if (DamageIncrease != 0)
        {
            string sign = DamageIncrease > 0 ? "+" : "";
            result += $"Damage {sign}{DamageIncrease}\n";
        }
        if (DamageAbsorbtion != 0)
        {
            string sign = DamageAbsorbtion > 0 ? "+" : "";
            result += $"Armor {sign}{DamageAbsorbtion}\n";
        }
        return result != "" ? result.Substring(0, result.Length - 1) : "";
    }
    //public uint DashBonus = 0;
}
public enum ItemClass { Sword, Shield, Armor, Amulet }
public enum ItemRarity { Common, Rare, Epic, Legendary }