using System.Collections;
using UnityEngine;

public class grenadeController : MonoBehaviour
{
    public Renderer grenadeRenderer;
    public AudioSource pulseAudioSource;
    public ParticleSystem explosionEffect;
    public LayerMask enemyLayer;
    private Coroutine detonationCoroutine;
    private bool isDetonating = false;

    public void ActivateGrenade(Renderer rend, AudioSource audioSrc)
    {
        grenadeRenderer = rend;
        pulseAudioSource = audioSrc;
        if (detonationCoroutine != null)
        {
            StopCoroutine(detonationCoroutine);
        }
        detonationCoroutine = StartCoroutine(DetonateAfterDelay());
    }

    IEnumerator DetonateAfterDelay()
    {
        float elapsed = 0f;
        Color originalColor = grenadeRenderer.material.color;
        while (elapsed < 5f)
        {
            // Interpolate color
            grenadeRenderer.material.color = Color.Lerp(originalColor, Color.red, Mathf.PingPong(elapsed * 2f, 1f));

            // Play audio
            pulseAudioSource.Play();
            Debug.Log("asdfasd");
            elapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        Detonate();
    }

    void Detonate()
    {
        // Stop the color pulse
        StopCoroutine(detonationCoroutine);

        // Play explosion effect
        explosionEffect.Play();

        // Detect enemies in the blast radius and handle them here
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionEffect.main.startSize.constantMax, enemyLayer);
        foreach (var hitCollider in hitColliders)
        {
            // Handle enemy hit by the explosion
            // Example: hitCollider.GetComponent<Enemy>().TakeDamage(damageAmount);
        }

        // Destroy the grenade object after the explosion (you might want to delay this if your explosion effect lasts longer)
        Destroy(gameObject, explosionEffect.main.duration);
    }

    public void StopDetonation()
    {
        if (detonationCoroutine != null)
        {
            StopCoroutine(detonationCoroutine);
            isDetonating = false;
        }
        // Reset the grenade color or any other property if needed
        grenadeRenderer.material.color = Color.white; // Assuming original color is white
    }

    public void ToggleDetonation()
    {
        if (isDetonating)
        {
            StopDetonation();
        }
        else
        {
            ActivateGrenade(grenadeRenderer, pulseAudioSource);
            isDetonating = true;
        }
    }
}
