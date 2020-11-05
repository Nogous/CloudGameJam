using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stomper : MonoBehaviour
{
    public int _Damage;
    public const float _StartPositionY = 2;
    public const float _StartStompY = 2.5f;
    public const float _EndStompY = 0;
    [ReadOnly] public DamageDealerType damageDealerType = DamageDealerType.Stomper;
    private bool startCooldown = false;
    public float cooldown = 7;
    private float cooldownRef;
    private Tween tween;
    public GameObject VFX;

    // Start is called before the first frame update
    void Start()
    {
        cooldownRef = cooldown;
        StartStomp();
        VFX.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.pause) { return; }

        if(startCooldown)
        {
            if(cooldown >= 0)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                StartStomp();
                startCooldown = false;
            }
        }

    }

    void StartStomp()
    {
        tween = this.transform.DOMoveY(_StartStompY, 1f)
        .OnComplete(() =>{
            tween = this.transform.DOMoveY(_EndStompY, 0.5f)
            .OnComplete(() => {
                VFX.SetActive(true);
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                tween = this.transform.DOMoveY(_StartPositionY, 1.5f)
                .OnComplete(() => {
                    this.gameObject.GetComponent<BoxCollider>().enabled = true;
                    cooldown = cooldownRef;
                    startCooldown = true;
                    VFX.SetActive(false);
                });
            });
        });
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Entity>() != null)
        {
            var _Entity = collision.gameObject.GetComponent<Entity>();

            if (_Entity.entitiesStats._Type == EntitiesStats.Type.Robot)
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(_Damage, damageDealerType);
            }
        }
    }
}
