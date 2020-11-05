using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [Expandable]
    public EntitiesStats entitiesStats;

    [Header("Current Stats")]
    //Stats Actual on this Entity
    [ReadOnly] public int health = 0;
    [ReadOnly] public int _CurrentDamage = 0;
    [ReadOnly] public float _CurrentMovementSpeed = 0;
    [ReadOnly] public float _CurrentAttackSpeed = 0;
    [ReadOnly] public int _CurrentHitRange = 0;
    [ReadOnly] public int shield = 0;
    [ReadOnly] public int giant = 0;
    [ReadOnly] public float focusScale = 1f;
    private Vector3 initScale;

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
    [ReadOnly] public Bonus[] _BonusPlayer = new Bonus[4];
    public float speedScale = 1f;

    private int jumpCount = 0;
    public float jumpHeight = 1f;
    private float jumpTarget = 0f;
    private float curentJump = 0f;
    public float jumpSpeed = 1f;

    private bool isMoving = true;
    
    // health
    private int maxHealth = 0;
    public Image healthBar;
    public Billboard billboard;

    private void Awake()
    {
        maxHealth = entitiesStats._HealthBase ;
        health = entitiesStats._HealthBase ;
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
        if (_WalkingPath.Count != 0)
            CheckPath();

        initScale = gameObject.transform.localScale;
        if (_WalkingPath.Count!= 0)
        _CurrentNextPositionPath = _WalkingPath[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.pause) { return; }
        Attack();
        Movement();

        UpdateJump();
        UpdateScale();
    }

    void Attack()
    {
        if (_EnnemyEntities.Count != 0 && entitiesStats._Type == EntitiesStats.Type.Tour)
        {
            if (_EnnemyEntities[0].health <= 0 && _EnnemyEntities[0] == null)
            {
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

                if (_EnnemyEntities[0].health -_CurrentDamage <= 0)
                {
                    _EnnemyEntities.Remove(_EnnemyEntities[0]);
                }

            }
        }
    }
    public void Jump()
    {
        jumpCount++;
        jumpTarget = jumpHeight;
    }

    public void EndJump()
    {
        jumpCount--;
        if (jumpCount<= 0)
        {
            jumpTarget = 0;
        }
    }

    private void UpdateJump()
    {
        if (curentJump < jumpTarget)
        {
            curentJump += jumpSpeed * Time.deltaTime;

            if (curentJump > jumpTarget)
            {
                curentJump = jumpTarget;
            }
        }
        else if (curentJump > jumpTarget)
        {
            curentJump -= jumpSpeed * Time.deltaTime;

            if (curentJump < jumpTarget)
            {
                curentJump = jumpTarget;
            }
        }

        transform.position += curentJump * Vector3.up;
    }

    private void UpdateScale()
    {
        if (transform.localScale.x < initScale.x * focusScale)
        {
            transform.localScale += Vector3.one * Time.deltaTime * speedScale;
            if (transform.localScale.x > initScale.x * focusScale)
            {
                transform.localScale = initScale * focusScale;
            }
        }
        else if (transform.localScale.x > initScale.x * focusScale)
        {
            transform.localScale -= Vector3.one * Time.deltaTime * speedScale;
            if (transform.localScale.x < initScale.x * focusScale)
            {
                transform.localScale = initScale * focusScale;
            }
        }
    }

    public void TakeDamage(int damage, DamageDealerType bulletType, GameObject damageDealerObject = null)
    {
        if (bulletType == DamageDealerType.Bullet)
        {
            if (shield == 1 )
            {
                this.health -= (int)(damage / 2);
            }
            else if (shield == 2)
            {
                this.health -= 0;
            }
            else
            {
                this.health -= (int)(damage);
            }
        }
        else if(bulletType == DamageDealerType.GiantBall)
        {
            int i = 0;

            for (int z = 0; z < _BonusPlayer.Length; z++)
            {
                if (_BonusPlayer[z] != null)
                {
                    if (_BonusPlayer[z].type == BonusType.Giant && _BonusPlayer[z].isActive)
                        i++;
                }
            }
            if (i == 0)
            {
                Debug.Log("Damage Deal !");
                this.health -= (int)(damage);
            }
            else if (i == 1)
            {
                Debug.Log("Damage Deal and get destroyed!");
                this.health -= (int)(damage);
                if (damageDealerObject != null)
                    Destroy(damageDealerObject, 0.1f);
            }
            else
            {
                Debug.Log("Damage don't deal and get destroyed !");
                if (damageDealerObject != null)
                    Destroy(damageDealerObject, 0.1f);
            }
        }
        else
        {
            this.health -= (int)(damage);
        }

        healthBar.fillAmount = (float)health / (float)maxHealth;

        if (health <= 0)
        {
            Death();
        }

    }

    void CheckPath()
    {
        _CurrentPos = 0;
        _StartPosition = this.transform.position;
        _CurrentNextPositionPath = _WalkingPath[_CurrentObjectID].transform.position;

        Vector3 difference = _CurrentNextPositionPath - transform.position;
        difference.Normalize();
        float rotationY = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    void Movement()
    {
        if (!isMoving) return;

        if (_WalkingPath.Count != 0 && entitiesStats._Type == EntitiesStats.Type.Robot)
        {
            transform.position += (_CurrentNextPositionPath - transform.position).normalized * _CurrentMovementSpeed * Time.deltaTime * 2;

            if ((_CurrentNextPositionPath - transform.position).magnitude <= 0.1f)
            {
                _CurrentObjectID++;
                if (_WalkingPath.Count > _CurrentObjectID)
                {
                    //Debug.Log(_CurrentObjectID +"/"+ _WalkingPath.Count);
                    _CurrentNextPositionPath = _WalkingPath[_CurrentObjectID].transform.position;
                    transform.forward = _CurrentNextPositionPath - transform.position;
                }
                else
                {
                    Death();
                }
            }

            /*
            Debug.Log(_StartPosition +" ; "+  _CurrentNextPositionPath);
            _CurrentPos += Time.deltaTime * _CurrentMovementSpeed / Vector3.Distance(_StartPosition, _CurrentNextPositionPath);


            this.transform.position = Vector3.Lerp(_StartPosition, _CurrentNextPositionPath, _AnimCurve.Evaluate(_CurrentPos));
            if (_CurrentPos >=1)
            {
                _CurrentObjectID++;
                CheckPath();
            }*/
        }
    }

    public void Death()
    {
        isMoving = true;
        Destroy(this.gameObject, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Entity>() != null)
        {
            if(other.gameObject.GetComponent<Entity>().entitiesStats._Type == EntitiesStats.Type.Robot)
            {
                if (this.entitiesStats._Type == EntitiesStats.Type.Tour)
                {
                    if (!_EnnemyEntities.Contains(other.gameObject.GetComponent<Entity>()))
                    {
                        _EnnemyEntities.Add(other.gameObject.GetComponent<Entity>());
                    }
                }
                else if(this.entitiesStats._Type == EntitiesStats.Type.Nexus)
                {
                    Debug.Log("Victory ! You destroy Nexus.");
                    if (UIManager.instance != null)
                        UIManager.instance.StatePanelVictory(true);
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
