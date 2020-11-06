using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Faster : Bonus
{
    [SerializeField] private float speedMultiplicator = 2f;

    protected override void V_ActiveBonus()
    {
        base.V_ActiveBonus();
        myEntity._CurrentMovementSpeed *= speedMultiplicator;
        AudioManager.Instance.Play("Speed");
    }

    protected override void EndBonus()
    {
        base.EndBonus();
        myEntity._CurrentMovementSpeed /= speedMultiplicator;
    }
}
