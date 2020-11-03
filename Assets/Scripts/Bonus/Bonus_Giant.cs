using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Giant : Bonus
{

    private void Reset()
    {
        this.type = BonusType.Giant;
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

        gameObject.GetComponent<Entity>()._CurrentHealth = gameObject.GetComponent<Entity>()._CurrentHealth * _Multiplier;
        this.gameObject.transform.localScale = this.gameObject.transform.localScale * _Multiplier;
    }

    public override void ActifCooldown()
    {
        base.ActifCooldown();
    }

    public override void DeactivateBonus()
    {
        base.DeactivateBonus();

        gameObject.GetComponent<Entity>()._CurrentHealth = gameObject.GetComponent<Entity>()._CurrentHealth / _Multiplier;
        this.gameObject.transform.localScale = this.gameObject.transform.localScale / _Multiplier;
    }
}
