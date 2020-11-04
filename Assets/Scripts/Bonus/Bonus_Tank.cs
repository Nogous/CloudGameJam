using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Tank : Bonus
{
    protected override void V_ActiveBonus()
    {
        base.V_ActiveBonus();
        myEntity.shield+=1;
    }

    protected override void EndBonus()
    {
        base.EndBonus();
        myEntity.shield -= 1;
    }
}
