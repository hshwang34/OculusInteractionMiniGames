using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public GameObject ballPrefab;     // Drag your ball prefab here
    public GameObject defaultSpawnPoint; // Default spawn point for the ball. You can set this in the inspector.

    private void Start()
    {
        if (!ballPrefab || !defaultSpawnPoint)
        {
            Debug.LogWarning("Please ensure both the ball prefab and the default spawn point are set.");
            return;
        }

        InstantiateBall();
    }

    // This public function can be called from other scripts to instantiate the ball at any specified location.
    public void InstantiateBall()
    {
        if (ballPrefab)
        {
            GameObject ballInstance = Instantiate(ballPrefab, defaultSpawnPoint.transform.position, Quaternion.identity);
            Rigidbody rb = ballInstance.GetComponent<Rigidbody>();

            if (rb)
            {
                // Initially, the ball should not be affected by gravity.
                rb.useGravity = false;
            }
        }
        else
        {
            Debug.LogError("Ball prefab is not assigned!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the ball has collided with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Destroy the ball 5 seconds after it collides with the ground
            Destroy(gameObject, 5f);
        }
    }

    // This function can be tied to a Unity Event, like a Button or an Input trigger.
    public void ActivateGravity()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = true;
        }
    }
}
