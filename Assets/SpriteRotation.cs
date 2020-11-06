using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    public void Rotate(Vector3 _rotation)
    {
        transform.Rotate(_rotation);
    }
}
