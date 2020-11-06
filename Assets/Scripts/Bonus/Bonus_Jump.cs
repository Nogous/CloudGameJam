using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Jump : Bonus
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void V_ActiveBonus()
    {
        base.V_ActiveBonus();
        myEntity.Jump();
    }

    protected override void EndBonus()
    {
        base.EndBonus();
        myEntity.EndJump();
    }
}
