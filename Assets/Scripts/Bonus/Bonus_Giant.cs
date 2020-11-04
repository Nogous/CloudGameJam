using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Giant : Bonus
{
    public float scaleMultiplicateur = 1.2f;
    public bool isGrowing = false;
    public bool isRapetic = false;

    protected override void V_ActiveBonus()
    {
        base.V_ActiveBonus();
        myEntity.giant += 1;
        myEntity.focusScale *= scaleMultiplicateur;
    }

    protected override void EndBonus()
    {
        base.EndBonus();
        myEntity.giant -= 1;
        myEntity.focusScale /= scaleMultiplicateur;
    }
}
