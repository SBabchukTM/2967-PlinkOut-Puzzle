using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity += new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), 0f);
        /*        if (Mathf.Abs(rb.velocity.y) < 0.5f)
                {
                    rb.velocity += new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), 0f);
                }*/
    }
}