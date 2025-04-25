using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask stopOnLayers;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Mirem si la layer del collider està inclosa a la LayerMask
        if (((1 << collision.gameObject.layer) & stopOnLayers) != 0) rb.bodyType = RigidbodyType2D.Static;
    }
}
