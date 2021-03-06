﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private GameObject objToMove;

    // Update is called once per frame
    void Update()
    {
        if (entity._EnnemyEntities.Count>0)
        {
            if (entity._EnnemyEntities[0] == null)
            {
                entity._EnnemyEntities.RemoveAt(0);
            }else
                objToMove.transform.forward = entity._EnnemyEntities[0].transform.position - transform.position;
        }
    }
}
