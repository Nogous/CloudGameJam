using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Entity entity;

    // Update is called once per frame
    void Update()
    {
        if (entity._EnnemyEntities.Count>0)
        {
            transform.forward = entity._EnnemyEntities[0].transform.position - transform.position;
        }
    }
}
