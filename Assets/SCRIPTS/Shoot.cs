using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ShootConfig
{
    public KeyCode key;              // Tecla que dispararà
    public GameObject projectile;    // Prefab que instanciarà
}

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform shootSpawnPoint;
    [SerializeField] private List<ShootConfig> shootConfigs = new List<ShootConfig>();
    [SerializeField] private float shootForce = 10f;

    private void Update()
    {
        HandleShooting();
    }

    //Input handler de tots els possibles inputs
    private void HandleShooting()
    {
        foreach (var config in shootConfigs)
        {
            if (Input.GetKeyDown(config.key))
            {
                ShootProjectile(config.projectile);
            }
        }
    }

    private void ShootProjectile(GameObject projectilePrefab)
    {
        if (projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, shootSpawnPoint.position, Quaternion.identity);

        // Si té Rigidbody2D, el disparem amb força
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null) 
        {
            float dirX = Mathf.Sign(transform.lossyScale.x); // +1 o -1 segons si està girat
            rb.velocity = new Vector2(dirX, 0f) * shootForce;
        }
    }
}
