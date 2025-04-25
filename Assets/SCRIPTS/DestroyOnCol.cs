using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCol : MonoBehaviour
{
    [SerializeField] private LayerMask destroyOnLayers;
    [SerializeField] private GameObject[] spawnOnCollide;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprovem si la layer del collider està dins del LayerMask
        if (((1 << other.gameObject.layer) & destroyOnLayers) != 0)
        {
            for (int i = 0; i < spawnOnCollide.Length; i++) Instantiate(spawnOnCollide[i], transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
