using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avatarBehavior : MonoBehaviour
{
    public Color hitColor = Color.red;     // Color to change to when hit
    public float hitDuration = 0.5f;       // Duration to display the hitColor

    private Renderer rend;                 // Reference to the Renderer component
    private Color originalColor;           // Original color to revert back to after hit

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is a projectile
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (rend != null)
            {
                // Change the color to the hitColor
                rend.material.color = hitColor;

                // Call the function to revert the color back after hitDuration
                Invoke("ResetColor", hitDuration);
            }
            // Optionally destroy the projectile upon hit
            // Destroy(collision.gameObject);
        }
    }

    private void ResetColor()
    {
        if (rend != null)
        {
            rend.material.color = originalColor;
        }
    }
}
