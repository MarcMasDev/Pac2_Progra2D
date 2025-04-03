using UnityEngine;

public class BikeFollower : MonoBehaviour
{
    public Transform Bike;

    private Vector3 offset;

    private void Start()
    {
        offset = Bike.transform.position - transform.position;
    }

    private void Update()
    {
        transform.position = Bike.position - offset;
    }
}
