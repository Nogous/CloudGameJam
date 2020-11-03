using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Expandable]
    public EntitiesStats entitiesStats;

    [Header("Current Stats")]
    //Stats Actual on this Entity
    [SerializeField] [ReadOnly] private int _CurrentHealth = 0;
    [SerializeField] [ReadOnly] private int _CurrentDamage = 0;
    [SerializeField] [ReadOnly] private float _CurrentMovementSpeed = 0;
    [SerializeField] [ReadOnly] private float _CurrentAttackSpeed = 0;
    [SerializeField] [ReadOnly] private int _CurrentHitRange = 0;

    [Header("Path factory to Nexus")]
    public List<GameObject> _WalkingPath;
    public AnimationCurve _AnimCurve;

    [Header("Ennemy to Attack")]
    public List<Entity> _EnnemyEntities;
    public GameObject _BulletPrefab;

    private float _CooldownHit = 0;
    private float _CurrentPos = 0;
    private int _CurrentObjectID = 0;

    Vector3 _StartPosition;
    Vector3 _CurrentNextPositionPath;

    [Header("Robot Bonus Bool")]
    public bool _SetGiant;
    public bool _SetFaster;
    public bool _SetTank;
    public bool _SetMirror;

    private void Awake()
    {
        _CurrentHealth = entitiesStats._HealthBase ;
        _CurrentDamage = entitiesStats._DamageBase ;
        _CurrentMovementSpeed = entitiesStats._MovementSpeedBase;
        _CurrentAttackSpeed = entitiesStats._AttackSpeedBase;
        if (this.gameObject.GetComponent<SphereCollider>() != null)
        {
            if (entitiesStats._HitRangeBase != this.gameObject.GetComponent<SphereCollider>().radius)
            {
                this.gameObject.GetComponent<SphereCollider>().radius = (float)entitiesStats._HitRangeBase;
                _CurrentHitRange = entitiesStats._HitRangeBase;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_SetGiant)
            SetGiant();

        if (_WalkingPath.Count != 0)
            CheckPath();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        Movement();
    }

    private void SetGiant()
    {
        _SetGiant = true;
        _CurrentHealth = _CurrentHealth * 2;
        this.gameObject.transform.localScale = this.gameObject.transform.localScale * 2;
    }

    void Attack()
    {
        if (_EnnemyEntities.Count != 0 && entitiesStats._Type == EntitiesStats.Type.Tour)
        {
            if (_EnnemyEntities[0]._CurrentHealth <= 0)
            {
                //_EnnemyEntities[0].Death();
                _EnnemyEntities.Remove(_EnnemyEntities[0]);
            }

            if (_CooldownHit > 0)
            {
                _CooldownHit -= Time.deltaTime;
            }
            else
            {
                _CooldownHit = _CurrentAttackSpeed;

                GameObject go = Instantiate(_BulletPrefab, this.gameObject.transform.position, Quaternion.identity);
                go.name = _BulletPrefab.name;
                go.GetComponent<Bullet>().SetDefaultVariable(_CurrentDamage, _EnnemyEntities[0].gameObject, _CurrentAttackSpeed);

                if (_EnnemyEntities[0]._CurrentHealth -_CurrentDamage <= 0)
                {
                    _EnnemyEntities.Remove(_EnnemyEntities[0]);
                }

                //_EnnemyEntities[0]._CurrentHealth -= this._CurrentDamage;

            }
        }
    }

    public void TakeDamage(int damage)
    {
        this._CurrentHealth -= damage;
        if (_CurrentHealth <= 0)
        {
            Death();
        }
    }

    void CheckPath()
    {
        _CurrentPos = 0;
        _StartPosition = this.transform.position;
        _CurrentNextPositionPath = _WalkingPath[_CurrentObjectID].transform.position;
    }

    void Movement()
    {
        if (_CurrentMovementSpeed > 0 && _WalkingPath.Count != 0 && entitiesStats._Type == EntitiesStats.Type.Robot)
        {
            if (_SetFaster) { _CurrentPos += Time.deltaTime * (_CurrentMovementSpeed * 2) / Vector3.Distance(_StartPosition, _CurrentNextPositionPath); }
            else { _CurrentPos += Time.deltaTime * _CurrentMovementSpeed / Vector3.Distance(_StartPosition, _CurrentNextPositionPath); }

            if(this.transform.position != _CurrentNextPositionPath)
            {
                this.transform.position = Vector3.Lerp(_StartPosition, _CurrentNextPositionPath, _CurrentPos < 1 ? _AnimCurve.Evaluate(_CurrentPos) : _CurrentPos);
            }
            else
            {
                if(_CurrentObjectID < _WalkingPath.Count - 1)
                {
                    _CurrentObjectID++;
                    CheckPath();
                }
            }

            //Mathf.Lerp(_WalkingPath[0].transform.position.magnitude, _WalkingPath[0 + 1].transform.position.magnitude, Time.deltaTime);
        }
    }

    public void Death()
    {
        Destroy(this.gameObject, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Entity>() != null)
        {
            if(other.gameObject.GetComponent<Entity>().entitiesStats._Type == EntitiesStats.Type.Robot)
            {
                if (!_EnnemyEntities.Contains(other.gameObject.GetComponent<Entity>()))
                {
                    _EnnemyEntities.Add(other.gameObject.GetComponent<Entity>());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            if (other.gameObject.GetComponent<Entity>().entitiesStats._Type == EntitiesStats.Type.Robot)
            {
                if (_EnnemyEntities.Contains(other.gameObject.GetComponent<Entity>()))
                {
                    _EnnemyEntities.Remove(other.gameObject.GetComponent<Entity>());
                }
            }
        }
    }
}
