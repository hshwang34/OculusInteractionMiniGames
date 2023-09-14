using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float lifetime = 10f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    public void FireBulletInDirection(Vector3 direction)
    {
        direction.y -= 0.5f;  // Adjust this value as needed to change how much lower you want the bullet to aim
        rb.velocity = direction.normalized * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // You can expand upon this depending on other interactions you want
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}