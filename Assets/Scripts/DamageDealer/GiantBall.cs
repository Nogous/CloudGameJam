using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBall : MonoBehaviour
{
    public int _Damage;
    public float _TimeToReachTarget;
    private Vector3 _StartPosition;
    private float ratio;
    public DamageDealerType damageDealerType = DamageDealerType.Bullet;

    [Header("Path")]
    public List<GameObject> _WalkingPath;
    public AnimationCurve _AnimCurve;
    Vector3 _CurrentNextPositionPath;
    private float _CurrentPos = 0;
    private int _CurrentObjectID = 0;

    // Start is called before the first frame update
    void Start()
    {
        _StartPosition = this.gameObject.transform.position;
        CheckPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.pause) { return; }

        //Debug.Log((_StartPosition - _CurrentNextPositionPath).normalized);

        //if ((_StartPosition - _CurrentNextPositionPath).normalized == Vector3.forward)
        //{
        //    transform.rotation = Quaternion.Euler(transform.rotation.x - Time.deltaTime * speed * 10, 0, 0);
        //}
        //else if ((_StartPosition - _CurrentNextPositionPath).normalized == Vector3.back)
        //{
        //    transform.rotation = Quaternion.Euler(transform.rotation.x + Time.deltaTime * speed * 10, 0, 0);
        //}
        //else if ((_StartPosition - _CurrentNextPositionPath).normalized == Vector3.right)
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.x - Time.deltaTime * speed);
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.x + Time.deltaTime * speed);
        //}

        _CurrentPos += Time.deltaTime * _TimeToReachTarget / Vector3.Distance(_StartPosition, _CurrentNextPositionPath);

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

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Entity>() != null)
        {
            var _Entity = collision.gameObject.GetComponent<Entity>();

            if (_Entity.entitiesStats._Type == EntitiesStats.Type.Robot)
            {
                int i = 0;

                for(int z = 0; z < _Entity._BonusPlayer.Length; z++)
                {
                    if(_Entity._BonusPlayer[z] != null)
                    {
                        if(_Entity._BonusPlayer[z].type == BonusType.Giant)
                            i++;
                    }
                }
                if (i == 1)
                {
                    Debug.Log("Damage Deal and get destroyed!");
                    collision.gameObject.GetComponent<Entity>().TakeDamage(_Damage, damageDealerType);
                    Destroy(this.gameObject, 0.1f);
                }
                else if (i == 0)
                {
                    Debug.Log("Damage Deal !");
                    collision.gameObject.GetComponent<Entity>().TakeDamage(_Damage, damageDealerType);
                }
                else
                {
                    Debug.Log("Damage don't deal and get destroyed !");
                    Destroy(this.gameObject, 0.1f);
                }


            }
        }
    }
}
