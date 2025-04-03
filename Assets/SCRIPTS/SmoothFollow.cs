using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float smoothSpeed = 1.0f;
    [SerializeField] private Vector2 offset;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //Late assegura que va després dels updates, d'aquesta forma primer el jugador es mou, i després la càmera el segueix a la posició
    private void LateUpdate()
    {
        if (target == null) return;


        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z); // Z was changed to 0 if I used Vector2
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

}
