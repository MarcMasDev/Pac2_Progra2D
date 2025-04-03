using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private int pointsPerEnemyKilled = 100;
    private void OnEnable()
    {
        HealthSystem.OnKilled += AddPoints;
    }

    private void OnDisable()
    {
        HealthSystem.OnKilled -= AddPoints;
    }

    private void AddPoints(HealthSystem healthSystem)
    {
        if (healthSystem.CompareTag("Enemy"))
        {
            Inventory.AddPoints(pointsPerEnemyKilled);
        }
    }
}
