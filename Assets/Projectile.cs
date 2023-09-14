using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 30f;
    public float lifeDuration = 5f;  // This is now the maximum time before the projectile will stick, in case it doesn't hit anything.

    private Rigidbody rb;
    private bool hasHit = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        hasHit = false;
    }

    void Update()
    {
        if (!hasHit)
        {
            // This assumes the projectile moves forward relative to its local orientation.
            //transform.position += transform.forward * speed * Time.deltaTime;

            lifeDuration -= Time.deltaTime;
            if (lifeDuration <= 0f)
            {
                StickToEnvironment();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            // Destroy the projectile when it hits the target.
            Destroy(gameObject);
        }
        else
        {
            StickToEnvironment();
        }
    }

    private void StickToEnvironment()
    {
        hasHit = true;
        rb.velocity = Vector3.zero; // Stop any movement
        rb.isKinematic = true;      // Prevent further physics interactions
        // You can also make the collider as a trigger if you want to avoid further collision detections.
    }
}