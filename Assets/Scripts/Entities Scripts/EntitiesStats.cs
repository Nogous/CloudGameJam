using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entities Stats", menuName = "Entities Stats/New Entity", order = 50)]
public class EntitiesStats : ScriptableObject
{
    public enum Type
    {
        None,
        Tour,
        Robot,
        Nexus,
    }

    public Type _Type;

    [Header("Stats")]
    public int _HealthBase;
    [Space(5)]
    public float _MovementSpeedBase;
    [Space(5)]
    public int _HitRangeBase;
    [Space(5)]
    public float _AttackSpeedBase;
    [Space(5)]
    public int _DamageBase;
}
