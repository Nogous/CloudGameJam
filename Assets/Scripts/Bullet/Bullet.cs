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
        if (GameManager.instance.pause) { return; }

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
                collision.gameObject.GetComponent<Entity>().TakeDamage(_Damage, bulletType);
                Destroy(this.gameObject, 0.1f);
            }
        }
    }
}
