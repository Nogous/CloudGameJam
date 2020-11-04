using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBall : MonoBehaviour
{
    [SerializeField] [ReadOnly] protected int _Damage;
    [SerializeField] [ReadOnly] protected GameObject _Target;
    private float _TimeToReachTarget;
    private Vector3 _StartPosition;
    private float ratio;
    public DamageDealerType damageDealerType = DamageDealerType.Bullet;

    [Header("Path")]
    public List<GameObject> _WalkingPath;
    public AnimationCurve _AnimCurve;
    Vector3 _CurrentNextPositionPath;
    private float _CurrentPos = 0;
    private int _CurrentObjectID = 0;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        _StartPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.pause) { return; }

        _CurrentPos += Time.deltaTime * speed / Vector3.Distance(_StartPosition, _CurrentNextPositionPath);

        if (this.transform.position != _CurrentNextPositionPath)
        {
            this.transform.position = Vector3.Lerp(_StartPosition, _CurrentNextPositionPath, _CurrentPos < 1 ? _AnimCurve.Evaluate(_CurrentPos) : _CurrentPos);
        }
        else
        {
            if (_CurrentObjectID < _WalkingPath.Count - 1)
            {
                _CurrentObjectID++;
                CheckPath();
            }
            if(_CurrentObjectID >= _WalkingPath.Count - 1)
            {
                _CurrentObjectID = 0;
                CheckPath();
            }
        }
    }

    void CheckPath()
    {
        _CurrentPos = 0;
        _StartPosition = this.transform.position;
        _CurrentNextPositionPath = _WalkingPath[_CurrentObjectID].transform.position;
    }

    public void SetDefaultVariable(int damage, GameObject target, float time)
    {
        _Damage = damage;
        _Target = target;
        _TimeToReachTarget = time;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Entity>() != null)
        {
            var _Entity = collision.gameObject.GetComponent<Entity>();

            if (_Entity.entitiesStats._Type == EntitiesStats.Type.Robot)
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(_Damage, damageDealerType);
                int i = 0;
                foreach(Bonus bonus in _Entity._BonusPlayer)
                {
                    if(bonus.type == BonusType.Giant)
                    {
                        i++;
                    }
                }
                if(i >= 2)
                {
                    Destroy(this.gameObject, 0.1f);
                }
            }
        }
    }
}
