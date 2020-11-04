using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BonusType {
    None,
    Giant,
    Faster,
    Tank,
    Mirror,

}

public class Bonus : MonoBehaviour
{
    [ReadOnly] public BonusType type;

    public bool isActif = false;

    public int _Multiplier = 2;

    public bool _IsCooldownNeeded = false;

    protected float cooldown;
    public float cooldownMax = 10f;

    protected virtual void Start()
    {
        cooldown = cooldownMax;
    }

    protected virtual void Update()
    {
        ActifCooldown();
    }
    
    public virtual void ActiveBonus()
    {
        isActif = true;
    }

    public virtual void ActifCooldown()
    {
       if(isActif && _IsCooldownNeeded)
        {
            if(cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                DeactivateBonus();
            }
        }
    }

    public virtual void DeactivateBonus()
    {
        cooldown = cooldownMax;
        isActif = false;
    }
}
