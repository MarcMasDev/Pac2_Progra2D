using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEnd : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float radius;

    private bool checkEnd = false;

    public Action OnEnd;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
            checkEnd = true;
        }
    }

    private void Update()
    {
        if (checkEnd)
        {
            DetectAndHandleHit();
        }
    }

    private void DetectAndHandleHit()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(endPoint.position, radius);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Player"))
            {
                StartCoroutine(End());
            }
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(2f);
        OnEnd?.Invoke();
    }
}
