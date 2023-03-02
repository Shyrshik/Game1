using UnityEngine;

public class Sword : Weapon
{
    public override float Attack()
    {
        return Target2D.Attack(this, Target2D.AttackPosition.Nearest, true);
    }
}
