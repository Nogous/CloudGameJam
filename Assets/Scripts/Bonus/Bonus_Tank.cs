using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Tank : Bonus
{
    public GameObject _prefabVFXShieldOn;
    public GameObject _VFXShield;

    protected override void V_ActiveBonus()
    {
        base.V_ActiveBonus();
        myEntity.shield+=1;
        _VFXShield = Instantiate(_prefabVFXShieldOn, myEntity.transform.parent);
    }

    protected override void EndBonus()
    {
        base.EndBonus();
        myEntity.shield -= 1;
        Destroy(_VFXShield, 0.1f);
    }
}
