using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] GameObject targetToDestroy = null;

    public void DestroyTarget()
    {
        Destroy(targetToDestroy);
    }
}
