using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public ItemRarity Rarity = ItemRarity.Common;
    public int DamageIncrease = 0;
    public float SwordLength = 1f;
    public int Armor = 0;
    public int DamageBlockage = 0;
    public int AdditionalAttackSpeedPercentage = 0;
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
        if (Armor != 0)
        {
            string sign = Armor > 0 ? "+" : "";
            result += $"Armor {sign}{Armor}\n";
        }
        if (DamageBlockage != 0)
        {
            string sign = DamageBlockage > 0 ? "+" : "";
            result += $"Block Strength {sign}{DamageBlockage}\n";
        }
        if (AdditionalAttackSpeedPercentage != 0)
        {
            string sign = AdditionalAttackSpeedPercentage > 0 ? "+" : "";
            result += $"Attack Speed {sign}{AdditionalAttackSpeedPercentage}%\n";
        }
        return result != "" ? result.Substring(0, result.Length - 1) : "";
    }
    //public uint DashBonus = 0;
}
public enum ItemClass { Sword, Shield, Armor, Amulet }
public enum ItemRarity { Common, Rare, Epic, Legendary }