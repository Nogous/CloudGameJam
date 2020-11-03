using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Mirror : Bonus
{

    private void Reset()
    {
        this.type = BonusType.Mirror;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void ActiveBonus()
    {
        base.ActiveBonus();
    }

    public override void ActifCooldown()
    {
        base.ActifCooldown();
    }

    public override void DeactivateBonus()
    {
        base.DeactivateBonus();
    }
}
