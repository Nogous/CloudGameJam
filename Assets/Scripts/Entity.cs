using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField][Expandable]
    private EntitiesStats entitiesStats;

    //Stats Actual on this Entity
    private int _CurrentHealth;
    private int _CurrentDamage;
    private int _CurrentMovementSpeed;
    private int _CurrentAttackSpeed;
    private int _CurrentHitRange;

    [Header("Path factory to Nexus")]
    public List<GameObject> _WalkingPath;

    [Header("Ennemy to Attack")]
    public List<Entity> _EnnemyEntities;

    private float cooldownHit = 0;

    private void Awake()
    {
        entitiesStats._HealthBase = _CurrentHealth;
        entitiesStats._DamageBase = _CurrentDamage;
        entitiesStats._MovementSpeedBase = _CurrentMovementSpeed;
        entitiesStats._AttackSpeedBase = _CurrentAttackSpeed;
        entitiesStats._HitRangeBase = _CurrentHitRange;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        Movement();
    }

    void Attack()
    {
        if (_EnnemyEntities != null)
        {
            if (cooldownHit > 0)
            {
                cooldownHit -= Time.deltaTime;
            }
            else
            {
                cooldownHit = _CurrentAttackSpeed;
                foreach (Entity entity in _EnnemyEntities)
                {
                    entity._CurrentHealth -= this._CurrentDamage;
                    if(entity._CurrentHealth <= 0) { entity.Death(); }
                }
            }
        }
    }

    void Movement()
    {
        if (_CurrentMovementSpeed > 0 && _WalkingPath != null)
        {
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
