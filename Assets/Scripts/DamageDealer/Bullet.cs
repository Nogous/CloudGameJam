using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] [ReadOnly] protected int _Damage;
    [SerializeField] [ReadOnly] protected GameObject _Target;
    private float _TimeToReachTarget;
    private Vector3 _StartPosition;
    private float ratio;
    public AnimationCurve _AnimCurve;
    [ReadOnly] public DamageDealerType damageDealerType = DamageDealerType.Bullet;

    // Start is called before the first frame update
    void Start()
    {
        _StartPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.pause) { return; }

        ratio += Time.deltaTime / _TimeToReachTarget;
        
        if (_Target == null)
        { Destroy(this.gameObject); }

        this.transform.position = Vector3.Lerp(_StartPosition, _Target != null ? _Target.transform.position : this.transform.position, ratio < 1 ? _AnimCurve.Evaluate(ratio) : ratio);
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
                collision.gameObject.GetComponent<Entity>().TakeDamage(_Damage, damageDealerType);
                Destroy(this.gameObject, 0.1f);
            }
        }
    }
}
