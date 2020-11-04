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

    public bool isActive = false;

    public float cooldown = 10f;
    [ReadOnly] [SerializeField] private float cooldownLogic;

    public float bonusDuration = 10f;
    [ReadOnly] [SerializeField] private float bonusDurationLogic;

    #region virtual
    protected virtual void Start()
    {
        cooldownLogic = 0;
    }
    
    protected virtual void V_ActiveBonus() { }

    protected virtual void UpdateBonus()
    {
        if (!isActive) return;
        bonusDurationLogic -= Time.deltaTime;
        if (bonusDurationLogic<= 0)
        {
            EndBonus();
        }
    }

    protected virtual void EndBonus()
    {
        isActive = false;
    }
    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActiveBonus();
        }

        CooldownReload();
        UpdateBonus();
    }
    private void ActiveBonus()
    {
        if (cooldownLogic > 0) return;

        cooldownLogic = cooldown;
        Debug.Log(cooldownLogic);
        bonusDurationLogic = bonusDuration;
        V_ActiveBonus();

        isActive = true;
    }
    private void CooldownReload()
    {
       if(!isActive)
        {
            if(cooldownLogic > 0)
            {
                cooldownLogic -= Time.deltaTime;
            }
        }
    }

    public void BonusCancel()
    {
    }
}
