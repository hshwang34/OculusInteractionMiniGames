using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject shootingPositionObject;
    public GameObject playerPositionObject;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPosition;
    public float shootingDelay = 3f;
    public float destructionDelay = 5f;

    private Transform shootingPosition;
  
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyState currentState;
    private int enemyHealth = 2;

    private enum EnemyState { Walking, Idling, Kneeling, Dying }

    private void Awake()
    {
        if (shootingPositionObject != null) shootingPosition = shootingPositionObject.transform;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = EnemyState.Walking;
        if (shootingPositionObject != null) shootingPosition = shootingPositionObject.transform;
        
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Walking:
                HandleWalkingState();
                break;
            case EnemyState.Idling:
            case EnemyState.Kneeling:
            case EnemyState.Dying:
                LookAtPlayer();
                break;
        }
    }

    public void SetShootingDestination(GameObject shootingPosition)
    {
        if (shootingPosition != null)
        {
            agent.SetDestination(shootingPosition.transform.position);
        }
    }

    private void HandleWalkingState()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            TransitionToState(EnemyState.Idling);
        }
    }

    private void TransitionToState(EnemyState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case EnemyState.Idling:
                StartCoroutine(ShootAtIntervals());
                break;
        }
    }

    private IEnumerator ShootAtIntervals()
    {
        while (currentState == EnemyState.Idling || currentState == EnemyState.Kneeling)
        {
            FireBullet();
            yield return new WaitForSeconds(shootingDelay);
        }
    }

    private void FireBullet()
    {
        animator.SetBool("ShotFired", true);
        Vector3 direction = playerPositionObject.transform.position - bulletSpawnPosition.position;  // Updated this line
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.LookRotation(direction));
        bullet.GetComponent<EnemyBulletBehaviour>().FireBulletInDirection(direction);
    }

    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = playerPositionObject.transform.position - transform.position;  // Updated this line
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        targetRotation *= Quaternion.Euler(0, 30f, 0);
        transform.rotation = targetRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (enemyHealth == 2)
            {
                TakeDamage(); // We assume that the TakeDamage function will handle state changes and animations.
            }
            else
            {
                animator.SetBool("IsHit2", true);
                TransitionToState(EnemyState.Dying);
                SetToKinematic();
                StartCoroutine(DestroyAfterDelay());
            }
            
        }
    }

    private void TakeDamage()
    {
        animator.SetBool("IsHit1", true);
        TransitionToState(EnemyState.Kneeling);
        enemyHealth--;
        //Destroy(gameObject, 5f); // Destroy after death animation
    }

    private void SetToKinematic()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destructionDelay);
        Destroy(gameObject);
    }
}
