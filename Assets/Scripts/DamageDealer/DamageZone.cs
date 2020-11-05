using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 10;
    public DamageDealerType damageDealerType = DamageDealerType.Trap;

    public float dps = 0.1f;
    private float dpsDuration = 0f;

    private List<Entity> entitys = new List<Entity>();

    private void Update()
    {
        dpsDuration -= Time.deltaTime;
        if (dpsDuration <= 0)
        {
            foreach (Entity item in entitys)
            {
                if (item == null)
                {
                    entitys.Remove(item);
                }
                else
                {
                    item.TakeDamage(damage, damageDealerType);
                    Debug.Log("hit");
                }
            }
            dpsDuration = 1/dps;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Entity>() != null)
        {
            var _Entity = collision.gameObject.GetComponent<Entity>();

            if (_Entity.entitiesStats._Type == EntitiesStats.Type.Robot)
            {
                entitys.Add(collision.gameObject.GetComponent<Entity>());
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            var _Entity = other.gameObject.GetComponent<Entity>();

            if (_Entity.entitiesStats._Type == EntitiesStats.Type.Robot)
            {
                entitys.Remove(other.gameObject.GetComponent<Entity>());
            }
        }
    }

    public void RemoveFromList(Entity entity)
    {
        entitys.Remove(entity);
    }
}
