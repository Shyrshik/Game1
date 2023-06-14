public class Sword : Weapon
{
    public override float Attack()
    {
        return Target2D.Attack.Nearest(this);
    }
}
