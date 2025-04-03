using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 0.5f;
    private void Awake() { Destroy(gameObject, timeToDestroy); }
}
