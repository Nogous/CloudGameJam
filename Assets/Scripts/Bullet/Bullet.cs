using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType{
    Classic,
    Laser,
}


public class Bullet : MonoBehaviour
{
    [SerializeField] [ReadOnly] private int _Damage;
    [SerializeField] [ReadOnly] private GameObject _Target;
    private int _Multiplier = 1;
    private float _TimeToReachTarget;
    private Vector3 _StartPosition;
    private float ratio;
    public AnimationCurve _AnimCurve;
    public BulletType bulletType;

    // Start is called before the first frame update
    void Start()
    {
        _StartPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ratio += Time.deltaTime / _TimeToReachTarget;

        this.transform.position = Vector3.Lerp(_StartPosition, _Target.transform.position, ratio < 1 ? _AnimCurve.Evaluate(ratio) : ratio);
    }

    public void SetDefaultVariable(int damage, GameObject target, float time)
    {
        _Damage = damage;
        _Target = target;
        _TimeToReachTarget = time;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Entity>() != null)
        {
            var _Entity = collision.gameObject.GetComponent<Entity>();

            if (_Entity.entitiesStats._Type == EntitiesStats.Type.Robot)
            {
                if (bulletType == BulletType.Classic)
                {
                    if (_Entity._BonusPlayer[0] != null)
                    {
                        if (_Entity._BonusPlayer[0].type == BonusType.Tank && _Entity._BonusPlayer[0].isActif)
                            _Multiplier = _Multiplier * 2;
                    }
                    if (_Entity._BonusPlayer[1] != null)
                    {
                        if (_Entity._BonusPlayer[1].type == BonusType.Tank && _Entity._BonusPlayer[1].isActif)
                            _Multiplier = _Multiplier * 2;
                    }
                    if (_Entity._BonusPlayer[2] != null)
                    {
                        if (_Entity._BonusPlayer[2].type == BonusType.Tank && _Entity._BonusPlayer[2].isActif)
                            _Multiplier = _Multiplier * 2;
                    }
                    if (_Entity._BonusPlayer[3] != null)
                    {
                        if (_Entity._BonusPlayer[3].type == BonusType.Tank && _Entity._BonusPlayer[3].isActif)
                            _Multiplier = _Multiplier * 2;
                    }

                    if (_Multiplier < 4)
                    {
                        collision.gameObject.GetComponent<Entity>().TakeDamage(_Damage / _Multiplier);
                    }
                    Destroy(this.gameObject, 0.1f);
                }
                else if (bulletType == BulletType.Laser)
                {
                    if (_Entity._BonusPlayer[0] != null)
                    {
                        if (_Entity._BonusPlayer[0].type == BonusType.Mirror && _Entity._BonusPlayer[0].isActif)
                        {
                            Destroy(this.gameObject, 0.1f);
                            return;
                        }
                    }
                    if (_Entity._BonusPlayer[1] != null)
                    {
                        if (_Entity._BonusPlayer[1].type == BonusType.Mirror && _Entity._BonusPlayer[1].isActif)
                        {
                            Destroy(this.gameObject, 0.1f);
                            return;
                        }
                    }
                    if (_Entity._BonusPlayer[2] != null)
                    {
                        if (_Entity._BonusPlayer[2].type == BonusType.Mirror && _Entity._BonusPlayer[2].isActif)
                        {
                            Destroy(this.gameObject, 0.1f);
                            return;
                        }
                    }
                    if (_Entity._BonusPlayer[3] != null)
                    {
                        if (_Entity._BonusPlayer[3].type == BonusType.Mirror && _Entity._BonusPlayer[3].isActif)
                        {
                            Destroy(this.gameObject, 0.1f);
                            return;
                        }
                    }
                    collision.gameObject.GetComponent<Entity>().TakeDamage(_Damage);
                }
            }
        }
    }
}
