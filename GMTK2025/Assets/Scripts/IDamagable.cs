public interface IDamagable
{
    public void RecieveDamage(uint damage, DamagableTeam source);
}
public enum DamagableTeam
{
    Enemy,
    Player
}