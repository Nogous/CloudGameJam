﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Jump : Bonus
{
    protected override void V_ActiveBonus()
    {
        base.V_ActiveBonus();
        myEntity.Jump();
    }
}
