using System;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    public float LinearSpeed = 1;
    public float RotationSpeed = 50;

    public Transform BackWheel;

    public Action OnKilled;
    public Action OnReachedEndOfLevel;

    private Rigidbody2D rb;
    private float wheelRadius;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        wheelRadius = GetComponent<CircleCollider2D>().radius;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveBackward();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveForward();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            DoStoppie();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            DoWheelie();
        }
    }

    private void MoveForward()
    {
        if (IsGrounded())
        {
            var right = transform.right;
            rb.velocity += new Vector2(right.x * LinearSpeed, right.y * LinearSpeed) * Time.deltaTime;
        }
    }

    private void MoveBackward()
    {
        if (IsGrounded())
        {
            var right = transform.right;
            rb.velocity -= new Vector2(right.x * LinearSpeed, right.y * LinearSpeed) * Time.deltaTime;
        }
    }

    private void DoWheelie()
    {
        rb.MoveRotation(rb.rotation + RotationSpeed * Time.fixedDeltaTime);
    }

    private void DoStoppie()
    {
        rb.MoveRotation(rb.rotation - RotationSpeed * Time.fixedDeltaTime);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircleAll(BackWheel.position, wheelRadius * 1.1f).Length > 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EndOfLevel"))
            OnReachedEndOfLevel?.Invoke();
        else
            OnKilled?.Invoke();
    }
}
