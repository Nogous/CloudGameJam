using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitiesStats", menuName = "EntitiesStats/New Entity", order = 50)]
public class EntitiesStats : MonoBehaviour
{
    public enum Type
    {
        Tour,
        Robot,
    }

    public Type _class;

    [Header("Stats")]
    public int _HealthBase;
    public int _SpeedBase;
}
