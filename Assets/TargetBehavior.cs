using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float moveDistance = 5f; // The distance the target will move from its starting position to either side.
    public Color disabledColor = Color.gray;

    private Vector3 startPosition;
    private bool isMovingRight = true;
    private Renderer rend;

    private void Start()
    {
        startPosition = transform.position;
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        MoveTarget();
    }

    private void MoveTarget()
    {
        float distanceMoved = transform.position.x - startPosition.x;

        // Switch direction when we've moved the max distance.
        if (Mathf.Abs(distanceMoved) >= moveDistance)
        {
            isMovingRight = !isMovingRight;
        }

        float moveDirection = isMovingRight ? 1f : -1f;

        // Move the target along the world's x-axis
        transform.position += new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);
    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Stop moving and change color when hit by a projectile.
            this.enabled = false; // Disabling the script stops the Update method and thereby stops the movement.
            rend.material.color = disabledColor;
        }
    }
}