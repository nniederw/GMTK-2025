using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public uint DamageIncrease = 0;
    public uint DamageAbsorbtion = 0;
    public Sprite Sprite;
    //public uint DashBonus = 0;
}