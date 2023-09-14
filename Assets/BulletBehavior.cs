using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float bulletLifetime = 5.0f;  // The bullet will be destroyed after 5 seconds.

    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Handle collision-related logic here. For example:
        // If the bullet should be destroyed upon impact, use:
        Destroy(gameObject);

        // Or if the bullet should embed, disable its Rigidbody and Collider so it doesn't move or cause further collisions.
        // Rigidbody rb = GetComponent<Rigidbody>();
        // if (rb) rb.isKinematic = true;
        // Collider col = GetComponent<Collider>();
        // if (col) col.enabled = false;
    }
}
