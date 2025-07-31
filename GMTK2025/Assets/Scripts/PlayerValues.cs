using UnityEngine;
public class PlayerValues : MonoBehaviour, IDamagable
{
    [SerializeField] private uint Health = 10;
    [SerializeField] private Item Sword = null;
    [SerializeField] private Item Armor = null;

    public void RecieveDamage(uint damage)
    {
        Health -= damage;
    }
    void Start()
    {

    }
    void Update()
    {

    }
}