using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Faster : Bonus
{
    [SerializeField] private float speedMultiplicator = 2f;
    private Entity myEntity;

    protected override void Start()
    {
        base.Start();
        myEntity = GetComponent<Entity>();
        if (myEntity == null)
        {
            Debug.LogError(gameObject + " ne contient pas d'entity");
            Destroy(this);
        }
    }

    protected override void V_ActiveBonus()
    {
        base.V_ActiveBonus();
        myEntity._CurrentMovementSpeed *= speedMultiplicator;
    }

    protected override void EndBonus()
    {
        base.EndBonus();
        myEntity._CurrentMovementSpeed /= speedMultiplicator;
    }
}
