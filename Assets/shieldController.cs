using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldController : MonoBehaviour
{
    public GameObject impactEffectPrefab; // Drag your particle effect prefab here in the Inspector

    public GameObject leftHandAnchorGO; // Drag your LeftHandAnchor GameObject here
    private bool shieldActive = false;

    private void Update()
    {
        if (shieldActive)
            FollowLeftHand();
    }


    public void ActivateShield()
    {
        shieldActive = true;
        this.gameObject.SetActive(true);
    }

    // This function will deactivate the entire shield GameObject
    public void DeactivateShield()
    {
        shieldActive = false;
        this.gameObject.SetActive(false);
    }

    private void FollowLeftHand()
    {

        if (leftHandAnchorGO != null)
        {
            Vector3 offsetDirection = -leftHandAnchorGO.transform.up;
            float distanceInFront = 0.5f;

            transform.position = leftHandAnchorGO.transform.position + offsetDirection * distanceInFront;
            transform.rotation = leftHandAnchorGO.transform.rotation;
            transform.Rotate(90f, 0f, 90f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Assuming your enemy bullet's tag is "EnemyBullet"
        {
            // Get the exact point of contact. This assumes there's only one point of contact.
            // If there might be multiple, you could use a loop or just take the first.
            Vector3 impactPoint = collision.contacts[0].point;

            // Instantiate and play the particle system
            PlayImpactEffect(impactPoint);
        }
    }

    private void PlayImpactEffect(Vector3 position)
    {
        // Instantiate the particle system at the position of impact
        GameObject effectInstance = Instantiate(impactEffectPrefab, position, Quaternion.identity);

        // Play the particle system
        effectInstance.GetComponent<ParticleSystem>().Play();

        // Optional: Automatically destroy the particle system after its duration to clean up
        // This assumes your particle system won't loop indefinitely
        Destroy(effectInstance, effectInstance.GetComponent<ParticleSystem>().main.duration);
    }
}