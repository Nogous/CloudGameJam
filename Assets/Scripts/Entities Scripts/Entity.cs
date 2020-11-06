using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    Vector3 lastPositionPath;
    Vector3 nextPositionPath;

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

    [Header("VFX / 3D Model")]
    public GameObject _prefabVFXDeath;
    public GameObject _armor1;
    public GameObject _armor2;
    public GameObject _armor4;
    public GameObject _robotFeet;
    public GameObject _speed1;
    public GameObject _speed2;

    // tower
    [Header("Tower")]
    [SerializeField] private Transform canon;

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
        if (_WalkingPath.Count != 0)
        {
            lastPositionPath = _WalkingPath[0].transform.position;
            nextPositionPath = _WalkingPath[1].transform.position;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.pause) { return; }
        Attack();
        Movement();

        UpdateJump();
        UpdateScale();
        UpdateModel();
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

                GameObject go = Instantiate(_BulletPrefab, canon.position, Quaternion.identity);
                go.name = _BulletPrefab.name;
                go.GetComponent<Bullet>().SetDefaultVariable(_CurrentDamage, _EnnemyEntities[0].gameObject, _CurrentAttackSpeed);

                if (_EnnemyEntities[0].health -_CurrentDamage <= 0)
                {
                    _EnnemyEntities.Remove(_EnnemyEntities[0]);
                }

            }
        }
    }

    public void UpdateModel()
    {
        if (entitiesStats._Type == EntitiesStats.Type.Robot)
        {
            if(_speed1 != null)
            {
                if (!_speed1.activeInHierarchy && _CurrentMovementSpeed == entitiesStats._MovementSpeedBase * 2)
                {
                    _speed1.SetActive(true);
                    _robotFeet.SetActive(false);
                }
                else if (_speed1.activeInHierarchy && _CurrentMovementSpeed == entitiesStats._MovementSpeedBase)
                {
                    _speed1.SetActive(false);
                    _robotFeet.SetActive(true);
                }
            }
            if (_speed2 != null)
            {
                if (!_speed2.activeInHierarchy && _CurrentMovementSpeed >= entitiesStats._MovementSpeedBase * 4)
                {
                    _speed2.SetActive(true);
                    _robotFeet.SetActive(false);
                }
                else if (_speed2.activeInHierarchy && _CurrentMovementSpeed == entitiesStats._MovementSpeedBase)
                {
                    _speed2.SetActive(false);
                    _robotFeet.SetActive(true);
                }
            }



            if (_armor1 != null)
            {
                if (!_armor1.activeInHierarchy && shield == 1)
                {
                    _armor1.SetActive(true);
                }
                else if (_armor1.activeInHierarchy && shield != 1 && shield != 3)
                {
                    _armor1.SetActive(false);
                }
            }
            if (_armor2 != null)
            {
                if (!_armor2.activeInHierarchy && shield == 2)
                {
                    _armor2.SetActive(true);
                }
                else if (_armor2.activeInHierarchy && shield != 2 && shield != 3)
                {
                    _armor2.SetActive(false);
                }
            }
            if (_armor1 != null && _armor2 != null)
            {
                if (shield == 3)
                {
                    _armor1.SetActive(true);
                    _armor2.SetActive(true);
                }
            }
            if (_armor1 != null && _armor2 != null && _armor4 != null)
            {
                if (shield == 4)
                {
                    _armor1.SetActive(true);
                    _armor2.SetActive(true);
                    _armor4.SetActive(true);
                }
                else { _armor4.SetActive(false); }
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
        lastPositionPath = _WalkingPath[_CurrentObjectID].transform.position;
        nextPositionPath = _WalkingPath[_CurrentObjectID+1].transform.position;

        Vector3 difference = nextPositionPath - transform.position;
        difference.Normalize();
        float rotationY = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        _CurrentMovementSpeed = entitiesStats._MovementSpeedBase / Vector3.Distance(lastPositionPath, nextPositionPath);
    }

    void Movement()
    {
        if (!isMoving) return;

        if (_WalkingPath.Count != 0 && entitiesStats._Type == EntitiesStats.Type.Robot)
        {
            /*
            transform.position += (nextPositionPath - transform.position).normalized * _CurrentMovementSpeed * Time.deltaTime * 2;

            if ((nextPositionPath - transform.position).magnitude <= 0.1f)
            {
                _CurrentObjectID++;
                if (_WalkingPath.Count > _CurrentObjectID)
                {
                    //Debug.Log(_CurrentObjectID +"/"+ _WalkingPath.Count);
                    nextPositionPath = _WalkingPath[_CurrentObjectID].transform.position;
                    transform.forward = nextPositionPath - transform.position;
                }
                else
                {
                    isMoving = true;
                    Debug.Log("Victory ! You destroy Nexus.");
                    if (UIManager.instance != null)
                        UIManager.instance.StatePanelVictory(true);
                    Destroy(this.gameObject, 0.1f);
                }
            }*/

            _CurrentPos += Time.deltaTime * _CurrentMovementSpeed;

            this.transform.position = Vector3.Lerp(lastPositionPath, nextPositionPath, _AnimCurve.Evaluate(_CurrentPos));
            if (_CurrentPos >=1)
            {
                _CurrentObjectID++;
                if (_CurrentObjectID>= _WalkingPath.Count-1)
                {
                    isMoving = false;
                    Debug.Log("Victory ! You destroy Nexus.");
                    if (UIManager.instance != null)
                        UIManager.instance.StatePanelVictory(true);


                    SceneManager.LoadScene(GameManager.instance.idNextScene);

                    Destroy(this.gameObject, 0.1f);
                }
                else
                CheckPath();
            }
        }
    }

    public void Death()
    {
        isMoving = false;
        GameObject go = Instantiate(_prefabVFXDeath, this.transform.position, Quaternion.identity);
        GameManager.instance.RobotDeath();
        Destroy(go, 0.5f);
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
                //else if(this.entitiesStats._Type == EntitiesStats.Type.Nexus)
                //{
                //    Debug.Log("Victory ! You destroy Nexus.");
                //    if (UIManager.instance != null)
                //        UIManager.instance.StatePanelVictory(true);
                //}
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
